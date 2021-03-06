﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    public Player player { get; private set; }
    public SlowTime slowTime { get; private set; }
    public Cannonball cannonball { get; private set; }
    public Fission fission { get; private set; }

    public Image healthbar;
    public Image meterTime;
    public Image meterCannon;
    public Image meterFission;
    public Image upgradeNote;

    bool FindPlayer() {
        if (!player) {
            player = FindObjectOfType<Player>();

            if (player) {
                slowTime = player.GetComponent<SlowTime>();
                cannonball = player.GetComponent<Cannonball>();
                fission = player.GetComponent<Fission>();
            }
        }

        return player;
    }
	
	// Update is called once per frame
	void Update () {
        if (FindPlayer()) {
            healthbar.fillAmount = player.health / player.actualMaxHealth;

            if (slowTime.slowed) {
                meterTime.fillAmount = slowTime.remainingRatio;
            } else {
                meterTime.fillAmount = slowTime.useTimer.chargeRatio;
            }

            meterCannon.fillAmount = cannonball.useTimer.chargeRatio;
            meterFission.fillAmount = fission.useTimer.chargeRatio;

            upgradeNote.enabled = Controller.inst.currentSave.getAvailablePoints() > 0;
        }
	}
}
