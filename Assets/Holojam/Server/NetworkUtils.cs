using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;
using ProtoBuf;
using update_protocol_v3;
using System.Threading;

namespace Holojam.Network {
	class PacketBuffer {
		public const int PACKET_SIZE = 65507; // ~65KB buffer sizes

		public byte[] bytes;
		public MemoryStream stream;
		public long frame;

		public PacketBuffer(int packetSize) {
			bytes = new byte[packetSize];
			stream = new MemoryStream(bytes);
			frame = 0;
		}

		public void copyFrom(PacketBuffer other) {
			this.bytes = other.bytes;
			this.stream = other.stream;
			this.frame = other.frame;
		}
	}

	[Obsolete("MasterStream/MasterServer is deprecated. Please use HolojamNetwork.")]
	class LiveObjectStorage {
		public static readonly Vector3 DEFAULT_VECTOR_POSITION = Vector3.zero;
		public static readonly Quaternion DEFAULT_QUATERNION_ROTATION = Quaternion.identity;

		public string label;
		public Vector3 position = DEFAULT_VECTOR_POSITION;
		public Quaternion rotation = DEFAULT_QUATERNION_ROTATION;
		public int bits = 0;
		public string blob = "";


		public LiveObjectStorage(string label) {
			this.label = label;
		}

		public LiveObject ToLiveObject() {
			LiveObject o = new LiveObject();
			o.label = this.label;

			o.x = position.x;
			o.y = position.y;
			o.z = position.z;

			o.qx = rotation.x;
			o.qy = rotation.y;
			o.qz = rotation.z;
			o.qw = rotation.w;

			o.button_bits = bits;
			
			if (!string.IsNullOrEmpty(blob)) {
				o.extra_data=blob;
			}

			return o;
		}
	}

	public class Motive{

		public enum Tag {
			HEADSET1,
			HEADSET2,
			HEADSET3,
			HEADSET4,
			HEADSET5,
			HEADSET6,
			HEADSET7,
			HEADSET8,
			HEADSET9,
			HEADSET10,
			HEADSET11,
			HEADSET12,
			HEADSET13,
			HEADSET14,
			HEADSET15,
			HEADSET16,
			HEADSET17,
			HEADSET18,
			HEADSET19,
			HEADSET20,
			HEADSET21,
			HEADSET22,
			HEADSET23,
			HEADSET24,
			HEADSET25,
			HEADSET26,
			HEADSET27,
			HEADSET28,
			HEADSET29,
			HEADSET30,
			WAND1, WAND2, WAND3, WAND4,
			BOX1, BOX2, SPHERE1,
			LEFTHAND1, RIGHTHAND1, LEFTFOOT1, RIGHTFOOT1,
			LEFTHAND2, RIGHTHAND2, LEFTFOOT2, RIGHTFOOT2,
			LEFTHAND3, RIGHTHAND3, LEFTFOOT3, RIGHTFOOT3,
			LEFTHAND4, RIGHTHAND4, LEFTFOOT4, RIGHTFOOT4,
			LAPTOP, TABLE,
			VIVE,VIVECONTROLLERLEFT,VIVECONTROLLERRIGHT
		}
		
		private static readonly Dictionary<Tag, string> tagNames = new Dictionary<Tag, string>() {
			{ Tag.HEADSET1, "VR1" },
			{ Tag.HEADSET2, "VR2" },
			{ Tag.HEADSET3, "VR3" },
			{ Tag.HEADSET4, "VR4" },
			{ Tag.HEADSET5, "VR5" },
			{ Tag.HEADSET6, "VR6" },
			{ Tag.HEADSET7, "VR7" },
			{ Tag.HEADSET8, "VR8" },
			{ Tag.HEADSET9, "VR9" },
			{ Tag.HEADSET10, "VR10" },
			{ Tag.HEADSET11, "VR11" },
			{ Tag.HEADSET12, "VR12" },
			{ Tag.HEADSET13, "VR13" },
			{ Tag.HEADSET14, "VR14" },
			{ Tag.HEADSET15, "VR15" },
			{ Tag.HEADSET16, "VR16" },
			{ Tag.HEADSET17, "VR17" },
			{ Tag.HEADSET18, "VR18" },
			{ Tag.HEADSET19, "VR19" },
			{ Tag.HEADSET20, "VR20" },
			{ Tag.HEADSET21, "VR21" },
			{ Tag.HEADSET22, "VR22" },
			{ Tag.HEADSET23, "VR23" },
			{ Tag.HEADSET24, "VR24" },
			{ Tag.HEADSET25, "VR25" },
			{ Tag.HEADSET26, "VR26" },
			{ Tag.HEADSET27, "VR27" },
			{ Tag.HEADSET28, "VR28" },
			{ Tag.HEADSET29, "VR29" },
			{ Tag.HEADSET30, "VR30" },
			{ Tag.WAND1, "VR1_wand" },
			{ Tag.WAND2, "VR2_wand" },
			{ Tag.WAND3, "VR3_wand" },
			{ Tag.WAND4, "VR4_wand" },
			{ Tag.BOX1, "VR1_box" },
			{ Tag.LEFTHAND1, "VR1_lefthand"},
			{ Tag.RIGHTHAND1, "VR1_righthand"},
			{ Tag.LEFTFOOT1, "VR1_leftankle"},
			{ Tag.RIGHTFOOT1, "VR1_rightankle"},
			{ Tag.LEFTHAND2, "VR2_lefthand"},
			{ Tag.RIGHTHAND2, "VR2_righthand"},
			{ Tag.LEFTFOOT2, "VR2_leftankle"},
			{ Tag.RIGHTFOOT2, "VR2_rightankle"},
			{ Tag.LEFTHAND3, "VR3_lefthand"},
			{ Tag.RIGHTHAND3, "VR3_righthand"},
			{ Tag.LEFTFOOT3, "VR3_leftankle"},
			{ Tag.RIGHTFOOT3, "VR3_rightankle"},
			{ Tag.LEFTHAND4, "VR4_lefthand"},
			{ Tag.RIGHTHAND4, "VR4_righthand"},
			{ Tag.LEFTFOOT4, "VR4_leftankle"},
			{ Tag.RIGHTFOOT4, "VR4_rightankle"},
			{ Tag.LAPTOP, "VR1_laptop"},
			{ Tag.TABLE, "VR1_table"},
			{ Tag.VIVE, "vive"},
			{ Tag.VIVECONTROLLERLEFT, "vive_controller_left"},
			{ Tag.VIVECONTROLLERRIGHT, "vive_controller_right"}
		};
		
		public static string GetName(Tag tag) {
			if (tagNames.ContainsKey(tag)) {
				return tagNames[tag];
			} else {
				throw new System.ArgumentException("Illegal tag.");
			}
		}
		public static int tagCount{get{return Enum.GetNames(typeof(Tag)).Length;}}
	}
}