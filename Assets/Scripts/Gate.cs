using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    public int index;
    public Sprite open;
    public SpriteRenderer sprite { get; private set; }
    public Collider2D collider { get; private set; }

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Controller.inst.currentSave.keys[index]) {
            collider.enabled = false;
            enabled = false;
            sprite.sprite = open;
        }
	}
}
