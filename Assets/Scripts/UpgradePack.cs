using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UpgradePack : MonoBehaviour {
    public int index = -1;
    public bool set = false;
    
	
	// Update is called once per frame
	void Update () {
        if (!Application.isPlaying && set) {
            index = FindObjectsOfType<UpgradePack>().Length - 1;
            set = false;
        }

        if (Application.isPlaying) {
            if (Controller.inst.currentSave.foundUpgradePoints != null && Controller.inst.currentSave.foundUpgradePoints[index]) {
                gameObject.SetActive(false);
            }
        }
		
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>()) {
            Controller.inst.currentSave.foundUpgradePoints[index] = true;
            Controller.inst.Save();
        }
    }
}
