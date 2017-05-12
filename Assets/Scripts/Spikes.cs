using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
        Player player = other.GetComponent<Player>();
        if (player) {
            player.Damage(player.health);
        }

    }
}
