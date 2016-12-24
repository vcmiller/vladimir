using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeView : MonoBehaviour {
    public Image back;
    public Image front;
    public Image select;
    public Image icon;
    public int index;
    public int requirement;

    public Button button { get; private set; }

    public bool purchased {
        get {
            return Controller.inst.currentSave.upgrades[index];
        }

        private set {
            Controller.inst.currentSave.upgrades[index] = value;
        }
    }
    private static UpgradeView[] views = new UpgradeView[16];

    void Start() {
        views[index] = this;

        button = GetComponent<Button>();
    }

    void Update() {
        front.enabled = purchased;
    }

    public void Toggle() {
        if (purchased) {
            Refund();
        } else {
            Purchase();
        }
    }

    public void Refund() {
        foreach (UpgradeView other in views) {
            if (other.requirement == index) {
                other.Refund();
            }
        }

        purchased = false;
    }

	public bool Purchase() {
        if (purchased) {
            return true;
        }

        if (requirement != -1 && !views[requirement].Purchase()) {
            return false;
        }

        if (Controller.inst.currentSave.getAvailablePoints() > 0) {
            purchased = true;
            return true;
        } else {
            return false;
        }
    }
}
