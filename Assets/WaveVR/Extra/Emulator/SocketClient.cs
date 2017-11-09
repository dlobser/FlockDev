// "WaveVR SDK 
// © 2017 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the WaveVR SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using UnityEngine;
using System.Collections;
using System.Threading;
using System;
using System.Net.Sockets;
using System.IO;
using proto;

/// @cond
namespace WaveVREmulator {
    class SocketClient : MonoBehaviour
    {
        private SocketManager phoneRemote;
        private Thread phoneEventThread;

        private volatile bool shouldStop = false;

        void OnDestroy()
        {
            shouldStop = true;

            if (phoneEventThread != null)
            {
                phoneEventThread.Join();
            }
        }

        private int blockingRead(NetworkStream stream, byte[] buffer, int index,int count)
        {
            int bytesRead = 0;
            while (!shouldStop && bytesRead < count)
            {
                try {
                    int n = stream.Read(buffer, index + bytesRead, count - bytesRead);
                    if (n <= 0) {
                        // Failed to read.
                        return -1;
                    }
                    bytesRead += n;
                } catch (IOException) {
                    // Read failed or timed out.
                    return -1;
                } catch (ObjectDisposedException) {
                    // Socket closed.
                    return -1;
                }
            }
            return bytesRead;
        }

        private int unpack32bits(byte[] array, int offset) {
            int num = 0;
            for (int i = 0; i < 4; i++) {
                num += array [offset + i] << (i * 8);
            }
            return num;
        }

        private static byte[] correctEndianness(byte[] array) {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(array);

            return array;
        }

        private const int kSocketReadTimeoutMillis = 5000;
        // Flag used to limit connection state logging to initial failure and successful reconnects.
        private volatile bool lastConnectionAttemptWasSuccessful = true;

        private void ProcessConnection(TcpClient tcpClient) {
            byte[] buffer = new byte[4];
            NetworkStream stream = tcpClient.GetStream();
            stream.ReadTimeout = kSocketReadTimeoutMillis;
            tcpClient.ReceiveTimeout = kSocketReadTimeoutMillis;
            while (!shouldStop) {
                int bytesRead = blockingRead(stream, buffer, 0, 4);
                if (bytesRead < 4) {
                    // Caught by phoneEventSocketLoop.
                    throw new Exception(
                        "Failed to read from controller emulator app event socket." +
                        "\nVerify that the controller emulator app is running.");
                }
                int msgLen = unpack32bits(correctEndianness(buffer), 0);

                byte[] dataBuffer = new byte[msgLen];
                bytesRead = blockingRead(stream, dataBuffer, 0, msgLen);
                if (bytesRead < msgLen) {
                    // Caught by phoneEventSocketLoop.
                    throw new Exception(
                        "Failed to read from controller emulator app event socket." +
                        "\nVerify that the controller emulator app is running.");
                }

                PhoneEvent proto =
                    PhoneEvent.CreateBuilder().MergeFrom(dataBuffer).Build();
                phoneRemote.OnPhoneEvent(proto);

                if (!lastConnectionAttemptWasSuccessful) {
                    Debug.Log("Successfully connected to controller emulator app.");
                    // Log first failure after above successful read from event socket.
                    lastConnectionAttemptWasSuccessful = true;
                }
            }
        }

        private static readonly int kPhoneEventPort = 7003;
        public bool connected { get; private set; }

        private void phoneConnect() {
            string addr = WIFI_IP;

            try {
                TcpClient tcpClient = new TcpClient(addr, kPhoneEventPort);
                connected = true;
                ProcessConnection(tcpClient);
                tcpClient.Close();
            } finally {
                connected = false;
            }
        }

        // Minimum interval, in seconds, between attempts to reconnect the socket.
        private const float kMinReconnectInterval = 1f;

        private void phoneEventSocketLoop() {
            while (!shouldStop) {
                long lastConnectionAttemptTime = DateTime.Now.Ticks;
                try {
                    phoneConnect();
                } catch(Exception e) {
                    if (lastConnectionAttemptWasSuccessful) {
                        Debug.LogWarningFormat("{0}\n{1}", e.Message, e.StackTrace);
                        // Suppress additional failures until we have successfully reconnected.
                        lastConnectionAttemptWasSuccessful = false;
                    }
                }

                // Wait a while in order to enforce the minimum time between connection attempts.
                TimeSpan elapsed = new TimeSpan(DateTime.Now.Ticks - lastConnectionAttemptTime);
                float toWait = kMinReconnectInterval - (float) elapsed.TotalSeconds;
                if (toWait > 0) {
                    Thread.Sleep((int) (toWait * 1000));
                }
            }
        }

        private string WIFI_IP = "192.168.0.2";

        public void Init(SocketManager remote) {
            phoneRemote = remote;

            phoneEventThread = new Thread(phoneEventSocketLoop);
            phoneEventThread.Start();

            ControllerEmulator ce = GameObject.Find ("Cube").GetComponent <ControllerEmulator>();
            if (ce != null)
            {
                WIFI_IP = ce.WIFI_IP;
                Debug.Log ("SocketClient::Init, WIFI_IP = " + WIFI_IP);
            }
        }
	}
}
/// @endcond