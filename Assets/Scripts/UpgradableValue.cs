using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradableValue<T>  {
    public T normal { get; private set; }
    public T upgraded { get; private set; }

    public int index { get; private set; }

    public bool isUpgraded {
        get {
            return Controller.inst.currentSave.upgrades[index];
        }
    }

    public UpgradableValue(T normal, T upgraded, int index) {
        this.normal = normal;
        this.upgraded = upgraded;
        this.index = index;
    }

    public static implicit operator T (UpgradableValue<T> val) {
        if (val.isUpgraded) {
            return val.upgraded;
        } else {
            return val.normal;
        }
    }
}
