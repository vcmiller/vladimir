using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    public Player player { get; private set; }

    public Slider healthbar;

    bool FindPlayer() {
        if (!player) {
            player = FindObjectOfType<Player>();
        }

        return player;
    }
	
	// Update is called once per frame
	void Update () {
        if (FindPlayer()) {
            healthbar.value = player.health / player.maxHealth;
        }
	}
}
