using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class ClearSave : MonoBehaviour {
    public bool clear;
    
	// Update is called once per frame
	void Update () {
		if (clear) {
            clear = false;
            File.Delete(Application.persistentDataPath + "/" + Controller.saveName);
        }
	}
}
