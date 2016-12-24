using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController inst { get; private set; }

    public PlayerUI playerHud { get; private set; }
    public Player player { get; private set; }
    public Ability[] abilities { get; private set; }
    public UpgradeUI upgradeUI { get; private set; }
    public bool upgradesOpen { get; private set; }

    void Awake() {
        inst = this;

        playerHud = FindObjectOfType<PlayerUI>();
        player = FindObjectOfType<Player>();
        abilities = player.GetComponents<Ability>();
        upgradeUI = FindObjectOfType<UpgradeUI>();

        upgradeUI.gameObject.SetActive(false);
        upgradesOpen = false;
    }

    void OpenUpgrades() {
        player.enabled = false;

        foreach (Ability abil in abilities) {
            abil.enabled = false;
        }

        Time.timeScale = 0.0f;
        playerHud.gameObject.SetActive(false);
        upgradeUI.gameObject.SetActive(true);
        upgradesOpen = true;
    }

    void CloseUpgrades() {

        foreach (Ability abil in abilities) {
            abil.enabled = true;
        }

        upgradesOpen = false;
        player.enabled = true;
        playerHud.gameObject.SetActive(true);
        upgradeUI.gameObject.SetActive(false);

        Controller.inst.Save();
    }

    void Update() {
        if (Input.GetButtonDown("Upgrades")) {
            if (upgradesOpen) {
                CloseUpgrades();
            } else {
                OpenUpgrades();
            }
        }
    }
}
