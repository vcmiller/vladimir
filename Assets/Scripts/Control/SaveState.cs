using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveState {
    public int checkpoint;
    public string file;
    public bool[] keys;
    public int upgradePoints;
    public bool[] foundUpgrades;

    public SaveState() {
        keys = new bool[8];
        foundUpgrades = new bool[8];
    }
}
