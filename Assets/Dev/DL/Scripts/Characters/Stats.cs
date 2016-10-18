using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//confirm server side only
//
public static class Stats : object {

	public static Dictionary<string,float[]> BugsEaten = new  Dictionary<string,float[]> ();

	public static void SetValue (string keyname, float value) {
		BugsEaten [keyname] = new float[]{ value, Time.time };
	}

	public static void SaveValues(){
		string output = "";
		foreach (string key in BugsEaten.Keys) {
			output += key + ":{";
			output += BugsEaten [key] [0] + "," + BugsEaten [key] [1] + "},";
		}
		System.IO.File.WriteAllText ("Assets/data" + "data" + ".txt", output);
		Debug.Log (output);
	}
}
