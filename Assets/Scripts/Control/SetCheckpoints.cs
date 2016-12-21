using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetCheckpoints : MonoBehaviour {
    public bool assign = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (assign) {
            assign = false;
            int index = 0;
            foreach (Checkpoint point in FindObjectsOfType<Checkpoint>()) {
                point.index = index++;
            }
        }
	}
}
