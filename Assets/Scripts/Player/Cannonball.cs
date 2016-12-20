using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : Ability {
    public float duration;
    public float speed;
    public float reticleDist;
    public Sprite sprite;

    public Vector2 aim { get; private set; }
    public bool active { get; private set; }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown(button) && !player.targetting && useTimer.canUse) {
            player.targetting = true;
        }

        if (Input.GetButtonUp(button) && player.targetting) {
            player.targetting = false;
            if (useTimer.Use) {
                Activate();
            }
        }

        if (Input.GetButton(button) && player.targetting) {
            Vector3 v = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (v.magnitude > 0.8f) {
                aim = v;
                player.reticle.transform.localPosition = aim.normalized * reticleDist;
                player.side = aim.x > 0;
            }
        }

        if (active) {
            player.rigidbody.velocity = aim.normalized * speed;
        }
	}

    void Activate() {
        Invoke("Deactivate", duration);

        player.rigidbody.bodyType = RigidbodyType2D.Dynamic;
        player.rigidbody.gravityScale = 0;
        active = true;

        player.animator.enabled = false;
        player.sprite.sprite = sprite;
    }

    void Deactivate() {
        player.rigidbody.velocity = Vector3.zero;
        player.rigidbody.gravityScale = 2;
        active = false;
        player.animator.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (active) {
            Enemy enemy = other.collider.GetComponent<Enemy>();
            if (enemy) {
                enemy.Die(transform.position.x < enemy.transform.position.x);
            }
        }
        
    }
}
