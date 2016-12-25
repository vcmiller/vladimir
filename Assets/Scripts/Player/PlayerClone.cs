using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClone : Player {
    protected override void Awake() {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        health = maxHealth;

        actualMaxHealth = new UpgradableValue<float>(maxHealth, maxHealthUp, Upgrade.generalHealth);
    }

    // Update is called once per frame
    protected override void Update() {
        UpdateGrounded();
        Attacks();

        Animations();
    }

    protected virtual bool ShouldAttack() {
        Vector2 dir = side ? Vector2.right : Vector2.left;
        float dist = grounded ? 1 : 5;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist);
        return hit.collider && hit.collider.GetComponent<Enemy>();
    }

    protected override void Attacks() {
        bool attack = false;
        if (grounded || canKick) {
            if (ShouldAttack()) {
                canKick = false;
                attack = true;
            }
        }

        if (attack) {
            attacking = true;
            if (grounded) {
            } else {
                rigidbody.velocity = side == RIGHT ? Vector2.right * 20 : Vector2.left * 20;
            }
            Invoke("StopAttack", 0.25f);
        }

        if (attacking) {
            Vector3 dir;
            if (side == RIGHT) {
                dir = Vector3.right;
            } else {
                dir = Vector3.left;
            }

            RaycastHit2D hit = Physics2D.CapsuleCast(transform.position, new Vector2(0.2f, 0.8f), CapsuleDirection2D.Vertical, 0, dir, 1f, int.MaxValue);
            if (hit.collider) {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy) {
                    enemy.Die(side);
                }
            }
        }
    }

    protected override void FixedUpdate() {
        Movement();
    }

    protected override void Movement() {
        running = true;
        transform.Translate((side == RIGHT ? Vector2.right : Vector2.left) * moveSpeed * Time.fixedDeltaTime);

        if (Controller.inst.currentSave.upgrades[Upgrade.fissionTurn] && Wall()) {
            side = !side;
        } else if (grounded && Controller.inst.currentSave.upgrades[Upgrade.fissionJump] && Gap()) {
            rigidbody.velocity = Vector2.up * jumpSpeed;
        }
    }

    bool Wall() {
        RaycastHit2D hit;
        if (side == RIGHT) {
            hit = Physics2D.CapsuleCast(transform.position, new Vector2(0.5f, 1.0f), CapsuleDirection2D.Vertical, 0, Vector3.right, 0.5f, int.MaxValue);
        } else {
            hit = Physics2D.CapsuleCast(transform.position, new Vector2(0.5f, 1.0f), CapsuleDirection2D.Vertical, 0, Vector3.left, 0.5f, int.MaxValue);
        }

        return hit.collider;
    }

    bool Gap() {
        RaycastHit2D hit;
        if (side == RIGHT) {
            hit = Physics2D.CapsuleCast(transform.position + Vector3.right * 0.5f, new Vector2(0.5f, 1.0f), CapsuleDirection2D.Vertical, 0, Vector3.down, 1.0f, int.MaxValue);
        } else {
            hit = Physics2D.CapsuleCast(transform.position + Vector3.left * 0.5f, new Vector2(0.5f, 1.0f), CapsuleDirection2D.Vertical, 0, Vector3.down, 1.0f, int.MaxValue);
        }

        return !hit.collider;
    }

    void Die() {
        Damage(health);
        Destroy(gameObject, 0.5f);
    }
}
