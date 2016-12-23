using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UpgradePack : MonoBehaviour {
    public int index = -1;

	// Use this for initialization
	void Start () {
		if (!Application.isPlaying && index < 0) {
            index = FindObjectsOfType<UpgradePack>().Length - 1;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.isPlaying) {
            if (Controller.inst.currentSave.foundUpgrades != null && Controller.inst.currentSave.foundUpgrades[index]) {
                print("HI");
                gameObject.SetActive(false);
            }
        }
		
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>()) {
            Controller.inst.currentSave.foundUpgrades[index] = true;
            Controller.inst.Save();
        }
    }
}
