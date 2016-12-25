using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour {
    public Selectable startSelect;
    public Text available;

	// Use this for initialization
	void OnEnable() {
		if (startSelect) {
            startSelect.Select();
        }
	}
	
	// Update is called once per frame
	void Update () {
        available.text = "POINTS: " + Controller.inst.currentSave.getAvailablePoints();
	}
}
