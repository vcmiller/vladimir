using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveState {
    public int checkpoint;
    public string file;
    public bool[] keys;
    public bool[] foundUpgradePoints;
    public bool[] upgrades;

    public int getPurchasedUpgrades() {
        int num = 0;
        foreach (bool b in upgrades) {
            if (b) {
                num++;
            }
        }

        return num;
    }
    
    public int getUpgradePoints() {
        int num = 0;
        foreach (bool b in foundUpgradePoints) {
            if (b) {
                num++;
            }
        }
        return num;
    }

    public int getAvailablePoints() {
        return getUpgradePoints() - getPurchasedUpgrades();
    }
}
