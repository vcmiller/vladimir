using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public Animator animator { get; private set; }
    public SpriteRenderer sprite { get; private set; }
    public int index;
    public int layer;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
	}

    void Close() {
        animator.SetBool("IsOpen", false);
    }

    void Open() {
        animator.SetBool("IsOpen", true);

        if (Controller.inst.currentSave.checkpoint != index) {
            Controller.inst.currentSave.checkpoint = index;
            Controller.inst.Save();
        }
    }

    void Update() {
        sprite.enabled = Controller.inst.activeLayers[layer];
    }

    void OnTriggerEnter2D(Collider2D other) {
        Player p = other.GetComponent<Player>();
        if (p) {

            foreach (Checkpoint point in FindObjectsOfType<Checkpoint>()) {
                point.Close();
            }

            Open();
        }
    }
}
