using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public Animator animator { get; private set; }
    public int index;

	// Use this for initialization
	void Start () {
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

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>()) {
            foreach (Checkpoint point in FindObjectsOfType<Checkpoint>()) {
                point.Close();
            }

            Open();
        }
    }
}
