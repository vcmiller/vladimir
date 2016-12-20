using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed;
    public float damage = 10;

    public Rigidbody2D rigidbody { get; private set; }

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.right * speed;
    }

    void OnCollisionEnter2D(Collision2D col) {
        Player p = col.collider.GetComponent<Player>();
        if (p) {
            p.Damage(damage);
            Destroy(gameObject);
        } else {
            rigidbody.gravityScale = 2;
            Destroy(gameObject, 0.5f);
        }

    }
}
