using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : Ability {
    public float duration;
    public float durationUp;

    public float stopBulletsCooldown = .1f;

    public UpgradableValue<float> actualDuration;
    public CooldownTimer stopBullets { get; private set; }

    public bool slowed { get; private set; }
    public float timeLastActivated { get; private set; }

    public SpriteRenderer deflectSprite;

    public override void Awake() {
        base.Awake();
        stopBullets = new CooldownTimer(stopBulletsCooldown);
        actualDuration = new UpgradableValue<float>(duration, durationUp, Upgrade.timeDuration);
    }

    public override void Update() {
        base.Update();

        if (Input.GetButtonDown(button)) {
            if (useTimer.Use) {
                player.slowTime = true;
                slowed = true;
                timeLastActivated = Time.time;
                Invoke("Cancel", actualDuration);
            } else if (slowed && Controller.inst.currentSave.upgrades[Upgrade.timeDeflect] && stopBullets.Use) {
                Invoke("StopDeflect", 0.02f);
                print("HI");
                deflectSprite.enabled = true;
                Projectile[] projectiles = FindObjectsOfType<Projectile>();
                foreach (Projectile proj in projectiles) {
                    if (Vector3.SqrMagnitude(proj.transform.position - transform.position) < 1.5f) {
                        proj.rigidbody.velocity = (proj.transform.position - transform.position).normalized * proj.speed;
                        proj.reflected = true;
                    }
                }
            }
        }
    }
    
    public float remainingRatio {
        get {
            if (!slowed) {
                return 0;
            } else {
                return 1 - ((Time.time - timeLastActivated) / actualDuration);
            }
        }
    }

    void StopDeflect() {
        deflectSprite.enabled = false;
    }

    void Cancel() {
        player.slowTime = false;
        slowed = false;
    }
}
