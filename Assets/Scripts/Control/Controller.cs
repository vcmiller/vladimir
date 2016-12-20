using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    public static Controller inst { get; private set; }

	// Use this for initialization
	void Start () {
        inst = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
