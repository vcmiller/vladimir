using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : Ability {
    public float duration;
    public float durationUp;
    public float speed;
    public float reticleDist;
    public Sprite sprite;
    public GameObject explosionPrefab;
    public SpriteRenderer shield;

    public UpgradableValue<float> actualDuration;

    public Vector2 aim { get; private set; }
    public bool active { get; private set; }

    public override void Awake() {
        base.Awake();
        aim = new Vector2(1, 0);
        actualDuration = new UpgradableValue<float>(duration, durationUp, Upgrade.cannonballDuration);
    }
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
		if (Input.GetButtonDown(button) && !player.targetting && useTimer.canUse && !active) {
            player.targetting = true;
        }

        if (Input.GetButtonUp(button) && player.targetting && !active) {
            player.targetting = false;
            if (useTimer.Use) {
                Activate();
            }
        }

        if (Input.GetButton(button) && player.targetting) {
            Vector3 v = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (v.magnitude > 0.8f) {
                aim = v;
                player.reticle.transform.localPosition = (Vector3)aim.normalized * reticleDist + Vector3.back * 2;
                player.side = aim.x > 0;
            }
        }

        if (active) {
            if (Controller.inst.currentSave.upgrades[Upgrade.cannonballBounce]) {
                player.rigidbody.velocity = player.rigidbody.velocity.normalized * speed;
            } else {
                player.rigidbody.velocity = aim.normalized * speed;
            }
        }

        if (Controller.inst.currentSave.upgrades[Upgrade.cannonballShield]) {
            shield.enabled = active;
        } else {
            shield.enabled = false;
        }
	}

    void Activate() {
        Invoke("Deactivate", actualDuration);

        player.rigidbody.bodyType = RigidbodyType2D.Dynamic;
        player.rigidbody.gravityScale = 0;
        player.rigidbody.velocity = aim.normalized * speed;
        active = true;

        player.animator.enabled = false;
        player.sprite.sprite = sprite;
    }

    void Deactivate() {
        player.rigidbody.velocity = Vector3.zero;
        player.rigidbody.gravityScale = 2;
        active = false;
        player.animator.enabled = true;

        if (Controller.inst.currentSave.upgrades[Upgrade.cannonballExplode]) {

            GameObject obj = Instantiate(explosionPrefab, transform.position + Vector3.back * 2, Quaternion.identity);
            Material m = obj.GetComponent<MeshRenderer>().material;
            m.SetFloat("_StartTime", Time.timeSinceLevelLoad);
            Destroy(obj, 1);

            foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
                if (Vector3.SqrMagnitude(transform.position - enemy.transform.position) < 9) {
                    enemy.Die(transform.position.x < enemy.transform.position.x);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (active) {
            Enemy enemy = other.collider.GetComponent<Enemy>();
            if (enemy) {
                enemy.Die(transform.position.x < enemy.transform.position.x);
            } else if (Input.GetButton(button) && Controller.inst.currentSave.upgrades[Upgrade.cannonballBounce]) {
                player.rigidbody.velocity = Vector2.Reflect(other.relativeVelocity, other.contacts[0].normal).normalized * speed;
            }
        }
        
    }
}
