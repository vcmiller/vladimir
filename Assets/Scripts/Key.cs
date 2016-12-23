using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    public int index;

	void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>()) {
            Controller.inst.currentSave.keys[index] = true;
            Controller.inst.Save();
        }
    }

    void Update() {
        if (Controller.inst.currentSave.keys[index]) {
            gameObject.SetActive(false);
        }
    }
}
