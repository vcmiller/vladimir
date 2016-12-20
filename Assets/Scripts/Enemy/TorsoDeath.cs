using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoDeath : DeathAnimation {
    public GameObject legsPrefab;
    public GameObject torsoPrefab;
    public float lifetime = 1.0f;

    public override void Play(bool side) {
        GameObject legs = Instantiate(legsPrefab, transform.position, Quaternion.identity);
        GameObject torso = Instantiate(torsoPrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = torso.GetComponent<Rigidbody2D>();
        
        if (side == RIGHT) {
            legs.transform.localScale = new Vector3(-1, 1, 1);
            torso.transform.localScale = new Vector3(-1, 1, 1);

            rb.angularVelocity = 360;
            rb.velocity = Vector2.right * 4 + Vector2.up * 2;
        } else {
            rb.angularVelocity = -360;
            rb.velocity = Vector2.left * 4 + Vector2.up * 2;
        }

        Destroy(gameObject);
        Destroy(legs, lifetime);
        Destroy(torso, lifetime);
    }
}
