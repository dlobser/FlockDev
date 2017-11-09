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
using System.Collections.Generic;
using wvr;
using WaveVR_Log;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Beam: MonoBehaviour
{
    public float startWidth = 0.001f;  // in x,y axis
    public float endWidth = 0.002f;  // let the bean seems the same width in far distance.
    public float startOffset = 0.003f;
    public float endOffset = 10;
    public int count = 3;
    public bool updateEveryFrame = false;
    public bool makeTail = true; // Offset from 0

    // Use this for initialization
    void OnEnable()
    {
        if (count < 3)
            count = 3;

        var meshfilter = GetComponent<MeshFilter>();
        if (meshfilter.mesh == null)
            meshfilter.mesh = createMesh();

        var meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
    }

    public List<Vector3> vertices;
    public List<Vector2> uvs;
    public List<Vector3> normals;
    public List<int> indices;
    public Vector3 position;

    public void Update()
    {
        if (!updateEveryFrame)
            return;
        if (count < 3)
            count = 3;
        var meshfilter = GetComponent<MeshFilter>();
        meshfilter.mesh = createMesh();
    }

    private Mesh createMesh()
    {
        Mesh mesh = new Mesh();
        int Count = count + 1;
        int verticesCount = Count * 2 + (makeTail ? 1 : 0);
        int indicesCount = Count * 6 + (makeTail ? count * 3 : 0);

        vertices = new List<Vector3>(verticesCount);
        uvs= new List<Vector2>(verticesCount);
        normals = new List<Vector3>(verticesCount);
        indices = new List<int>(indicesCount);

        Matrix4x4 mat = new Matrix4x4();

        for (int i = 0; i < Count; i++)
        {
            int angle = (int) (i * 360.0f / count);
            // make rotation matrix
            mat.SetTRS(new Vector3(0, 0, 0), Quaternion.AngleAxis(angle, new Vector3(0, 0, 1)), new Vector3(1, 1, 1));

            // start
            vertices.Add(mat.MultiplyVector(new Vector3(0, startWidth, startOffset)));
            uvs.Add(new Vector2(0.5f,0.5f));
            normals.Add(mat.MultiplyVector(new Vector3(0, 1, 0)).normalized);

            // end
            vertices.Add(mat.MultiplyVector(new Vector3(0, endWidth, endOffset)));
            Vector2 uv = mat.MultiplyVector(new Vector3(0, 0.5f, 0));
            uv.x = uv.x + 0.5f;
            uv.y = uv.y + 0.5f;
            uvs.Add(uv);
            normals.Add(mat.MultiplyVector(new Vector3(0, 1, 0)).normalized);
        }

        for (int i = 0; i < count; i++)
        {
            // bd
            // ac
            int a, b, c, d;
            a = i * 2;
            b = i * 2 + 1;
            c = i * 2 + 2;
            d = i * 2 + 3;

            // first
            indices.Add(a);
            indices.Add(d);
            indices.Add(b);

            // second
            indices.Add(a);
            indices.Add(c);
            indices.Add(d);
        }

        // Make Tail
        if (makeTail)
        {
            vertices.Add(new Vector3(0, 0, 0));
            uvs.Add(new Vector2(0.5f, 0.5f));
            normals.Add(new Vector3(0, 0, 0));
            int tailIdx = count * 2;
            for (int i = 0; i < count; i++)
            {
                int idx = i * 2;

                indices.Add(tailIdx);
                indices.Add(idx + 2);
                indices.Add(idx);
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.SetUVs(0, uvs);
        mesh.SetUVs(1, uvs);
        mesh.normals = normals.ToArray();
        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
        mesh.name = "Beam";
        return mesh;
    }
}
