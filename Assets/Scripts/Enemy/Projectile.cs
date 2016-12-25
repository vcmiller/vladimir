using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed;
    public float damage = 10;

    public bool reflected { get; set; }

    public Rigidbody2D rigidbody { get; private set; }

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.right * speed;
    }

    void OnCollisionEnter2D(Collision2D col) {
        Player p = col.collider.GetComponent<Player>();
        Enemy e = col.collider.GetComponent<Enemy>();
        if (p) {
            Cannonball c = p.GetComponent<Cannonball>();
            if (c && c.active && Controller.inst.currentSave.upgrades[Upgrade.cannonballShield]) {
                Destroy(gameObject);
            } else {
                p.Damage(damage);
                Destroy(gameObject);
            }
        } else if (reflected && e) {
            e.Die(transform.position.x < e.transform.position.x);
            Destroy(gameObject);
        } else {
            rigidbody.gravityScale = 2;
            Destroy(gameObject, 0.5f);
        }



    }
}
