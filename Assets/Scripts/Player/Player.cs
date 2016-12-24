using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public Animator animator { get; protected set; }
    public Rigidbody2D rigidbody { get; protected set; }
    public SpriteRenderer sprite { get; protected set; }
    public SpriteRenderer reticle { get; protected set; }
    public Camera camera { get; protected set; }

    public bool side { get; set; }
    public bool running { get; protected set; }
    public bool jumpHeld { get; protected set; }
    public bool attacking { get; protected set; }
    public bool grounded { get; protected set; }
    public float health { get; protected set; }
    public bool targetting { get; set; }
    public bool slowTime { get; set; }
    public bool canKick { get; protected set; }
    
    public float runThreshold = 0.05f;
    public float moveSpeed = 5;
    public float moveSpeedUp = 7;
    public float jumpSpeed = 10;
    public float jumpSpeedUp = 13;
    public float jumpThreshold = 0.2f;
    public Vector2 cameraRange;
    public float maxHealth = 100;
    public float maxHealthUp = 130;
    public float wallJumpRange = .5f;
    public Vector2 wallJumpSpeed;
    public float hmove = 0;

    public const bool LEFT = false;
    public const bool RIGHT = true;

    public UpgradableValue<float> actualMaxHealth;
    public UpgradableValue<float> actualMoveSpeed;
    public UpgradableValue<float> actualJumpSpeed;

    protected virtual void Awake() {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        camera = GetComponentInChildren<Camera>();
        reticle = transform.FindChild("Reticle").GetComponent<SpriteRenderer>();
        health = maxHealth;

        actualMaxHealth = new UpgradableValue<float>(maxHealth, maxHealthUp, Upgrade.generalHealth);
        actualMoveSpeed = new UpgradableValue<float>(moveSpeed, moveSpeedUp, Upgrade.generalMove);
        actualJumpSpeed = new UpgradableValue<float>(jumpSpeed, jumpSpeedUp, Upgrade.generalJump);
	}

    public virtual void Damage(float damage) {
        health -= damage;
        ClampHealth();
        if (health == 0) {
            GetComponent<DeathAnimation>().Play(side);
        }
    }

    protected virtual void OnGUI() {
        if (targetting || slowTime) {
            GUI.color = new Color(0, 1, 0, 0.1f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }
    }

    protected virtual void ClampHealth() {
        health = Mathf.Clamp(health, 0, actualMaxHealth);
    }

    // Update is called once per frame
    protected virtual void Update () {
        if (!targetting) {
            UpdateGrounded();
            Attacks();
        }

        Animations();
        UpdateCamera();

        reticle.enabled = targetting;
        rigidbody.bodyType = targetting ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;

        float targetTime = 1.0f;

        if (targetting) {
            targetTime = 0.1f;
        } else if (slowTime) {
            if (Controller.inst.currentSave.upgrades[Upgrade.timeEffect]) {
                targetTime = 0.1f;
            } else {
                targetTime = 0.15f;
            }
        }
        
        Time.timeScale = Mathf.MoveTowards(Time.timeScale, targetTime, Time.unscaledDeltaTime * 4);

        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        ClampHealth();
    }

    protected virtual void FixedUpdate() {
        if (!targetting) {
            Movement();
        }
    }

    protected virtual void UpdateGrounded() {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.1f, Vector2.down, 0.51f);
        grounded = hit.collider;

        if (grounded) {
            canKick = true;
        }
    }

    protected virtual void Attacks() {
        bool attack = false;
        if (grounded || canKick) {
            if (Input.GetButtonDown("AttackLeft") && !attacking) {
                side = LEFT;
                attack = true;

                canKick = false;
            } else if (Input.GetButtonDown("AttackRight") && !attacking) {
                side = RIGHT;
                attack = true;

                canKick = false;
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

    void OnCollision2D(Collider2D other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy) {
            if (side == (enemy.transform.position.x > transform.position.x) && attacking) {
                enemy.Die(side);
            } else if (rigidbody.velocity.y < 0) {
                enemy.Die(side);
            }
        }
    }

    protected virtual void UpdateCamera() {
        Vector3 v = new Vector3(Input.GetAxis("Horizontal2") * cameraRange.x, Input.GetAxis("Vertical2") * cameraRange.y);
        camera.transform.localPosition = new Vector3(v.x, v.y, -4);
    }

    protected virtual void StopAttack() {
        attacking = false;
        Vector2 v = rigidbody.velocity;
        v.x = 0;
        rigidbody.velocity = v;
    }

    protected virtual void Movement() {
        float f = Input.GetAxis("Horizontal");

        hmove = f * actualMoveSpeed;
        if (Controller.inst.currentSave.upgrades[Upgrade.timeMove] && slowTime) {
            hmove *= 2f;
        }

        transform.Translate(Vector3.right * hmove * Time.fixedDeltaTime);

        if (!attacking) {
            if (f < -runThreshold) {
                side = LEFT;
            } else if (f > runThreshold) {
                side = RIGHT;
            }
        }


        running = Mathf.Abs(f) > runThreshold;

        if (Input.GetAxis("Vertical") > jumpThreshold && !jumpHeld) {
            if (grounded) {
                rigidbody.velocity = Vector2.up * actualJumpSpeed;
            } else {
               // TryWallJump();
            }
        }

        if (Input.GetAxis("Vertical") < jumpThreshold) {
            jumpHeld = false;
        } else {
            jumpHeld = true;
        }
    }

    void TryWallJump() {
        RaycastHit2D left = Physics2D.Raycast(transform.position, Vector3.left, wallJumpRange);
        RaycastHit2D right = Physics2D.Raycast(transform.position, Vector3.right, wallJumpRange);

        if (left.collider && right.collider) {
            if (left.distance < right.distance) {
                WallJump(LEFT);
            } else {
                WallJump(RIGHT);
            }
        } else if (left.collider) {
            WallJump(LEFT);
        } else if (right.collider) {
            WallJump(RIGHT);
        }
    }

    void WallJump(bool side) {
        print("WalJJUMP");
        rigidbody.velocity = (side ? Vector2.left : Vector2.right) * wallJumpSpeed.x + Vector2.up * wallJumpSpeed.y;
    }

    protected virtual void Animations() {
        sprite.transform.localScale = new Vector3(side == RIGHT ? 1 : -1, 1, 1);
        animator.SetBool("Running", running);
        animator.SetBool("Punching", attacking);
        animator.SetBool("Grounded", grounded);
    }
}
