using UnityEngine;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MergeMesh : MonoBehaviour {
	public string outputPath = "Assets/mesh.asset";
	void Start() {
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		int i = 0;
		while (i < meshFilters.Length) {
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			//Compiler warns this is obsolete and recoomends the fllowing statement.
//			meshFilters[i].gameObject.active = false;
			meshFilters[i].gameObject.SetActive(false);
			i++;
		}
		transform.GetComponent<MeshFilter>().mesh = new Mesh();
		transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		AssetDatabase.CreateAsset( transform.GetComponent<MeshFilter>().mesh, outputPath);
		AssetDatabase.SaveAssets();
		//Compiler finds this statement obsolete, substitute follows.
//		transform.gameObject.active = true;
		transform.gameObject.SetActive(true);
	}
}