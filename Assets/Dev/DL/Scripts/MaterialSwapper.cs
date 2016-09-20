using UnityEngine;
using System.Collections;

public class MaterialSwapper : MonoBehaviour {

	public Material[] materials;
	public int whichMaterial;
	public MeshRenderer rend;

	public void swapMat(){
		whichMaterial++;
		if (whichMaterial > materials.Length)
			whichMaterial = 0;
		rend.material = materials [whichMaterial];
	}

	public void swapMat(int which){
		whichMaterial = which;
		if (whichMaterial > materials.Length)
			whichMaterial = 0;
		rend.material = materials [whichMaterial];
	}

}
