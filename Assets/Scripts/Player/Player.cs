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
    
    public float runThreshold = 0.05f;
    public float moveSpeed = 0.5f;
    public float jumpSpeed = 2;
    public float jumpThreshold = 0.2f;
    public Vector2 cameraRange;
    public float maxHealth = 100;

    public const bool LEFT = false;
    public const bool RIGHT = true;

    
	protected virtual void Awake() {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        camera = GetComponentInChildren<Camera>();
        reticle = transform.FindChild("Reticle").GetComponent<SpriteRenderer>();
        health = maxHealth;
	}

    public virtual void Damage(float damage) {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

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
        
        Time.timeScale = Mathf.MoveTowards(Time.timeScale, targetting || slowTime ? 0.1f : 1.0f, Time.unscaledDeltaTime * 4);

        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    protected virtual void FixedUpdate() {
        if (!targetting) {
            Movement();
        }
    }

    protected virtual void UpdateGrounded() {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.1f, Vector2.down, 0.51f);
        grounded = hit.collider;
    }

    protected virtual void Attacks() {
        bool attack = false;
        if (Input.GetButtonDown("AttackLeft") && !attacking) {
            side = LEFT;
            attack = true;
        } else if (Input.GetButtonDown("AttackRight") && !attacking) {
            side = RIGHT;
            attack = true;
        }

        if (attack) {
            attacking = true;
            if (grounded) {
                Invoke("StopAttack", 0.25f);
            } else {
                rigidbody.velocity = side == RIGHT ? Vector2.right * 20 : Vector2.left * 20;
                Invoke("StopAttack", 0.3f);
            }

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

        transform.Translate(Vector3.right * f * moveSpeed * Time.fixedDeltaTime);

        if (!attacking) {
            if (f < -runThreshold) {
                side = LEFT;
            } else if (f > runThreshold) {
                side = RIGHT;
            }
        }


        running = Mathf.Abs(f) > runThreshold;

        if (grounded && (Input.GetButtonDown("Jump") || (Input.GetAxis("Vertical") > jumpThreshold && !jumpHeld))) {
            rigidbody.velocity = Vector2.up * jumpSpeed;
        }

        if (Input.GetAxis("Vertical") < jumpThreshold) {
            jumpHeld = false;
        } else {
            jumpHeld = true;
        }
    }

    protected virtual void Animations() {
        sprite.transform.localScale = new Vector3(side == RIGHT ? 1 : -1, 1, 1);
        animator.SetBool("Running", running);
        animator.SetBool("Punching", attacking);
        animator.SetBool("Grounded", grounded);
    }
}
