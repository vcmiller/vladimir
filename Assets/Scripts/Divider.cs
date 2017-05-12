using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divider : MonoBehaviour {
    public int layerTop;
    public int layerBottom;

    public SpriteRenderer sprite { get; private set; }

	// Use this for initialization
	void Start () {
        sprite = transform.FindChild("Gradient").GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        bool top = Controller.inst.activeLayers[layerTop];
        bool bottom = Controller.inst.activeLayers[layerBottom];
        if (top == bottom) {
            sprite.enabled = false;
        } else if (top) {
            sprite.enabled = true;
            sprite.transform.localScale = new Vector3(1, 1, 1);
        } else if (bottom) {
            sprite.enabled = true;
            sprite.transform.localScale = new Vector3(1, -1, 1);
        }
	}
}
