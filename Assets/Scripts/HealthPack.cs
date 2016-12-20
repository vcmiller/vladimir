using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {
    public float healing = 30;
    public float regenTime = 30;

	void OnTriggerEnter2D(Collider2D col) {
        Player player = col.GetComponent<Player>();
        if (player && player.health < player.maxHealth) {
            player.Damage(-healing);
            gameObject.SetActive(false);
            Invoke("ReEnable", regenTime);
        }
    }

    void ReEnable() {
        gameObject.SetActive(true);
    }
}
