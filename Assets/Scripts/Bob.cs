using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour {
    public float amplitude;
    public float period;

    public Vector3 startPosition { get; private set; }

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = startPosition + Vector3.up * amplitude * Mathf.Sin(Time.time * 2 * Mathf.PI / period);
	}
}
