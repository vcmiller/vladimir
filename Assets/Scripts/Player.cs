using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public Animator animator { get; private set; }
    public Rigidbody2D rigidbody { get; private set; }
    public Transform sprite { get; private set; }
    public Camera camera;

    public bool side { get; private set; }
    public bool running { get; private set; }
    public bool jumpHeld { get; private set; }
    public bool attacking { get; private set; }
    public bool grounded { get; private set; }
    
    public float runThreshold = 0.05f;
    public float moveSpeed = 0.5f;
    public float jumpSpeed = 2;
    public float jumpThreshold = 0.2f;
    public Vector2 cameraRange;

    const bool LEFT = false;
    const bool RIGHT = true;

    
	void Awake() {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>().transform;
        camera = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateGrounded();
        Attacks();
        Animations();
        UpdateCamera();
    }

    void FixedUpdate() {
        Movement();
    }

    void UpdateGrounded() {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.1f, Vector2.down, 0.51f);
        grounded = hit.collider;
    }

    void Attacks() {
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
    }

    void UpdateCamera() {
        Vector3 v = new Vector3(Input.GetAxis("Horizontal2") * cameraRange.x, Input.GetAxis("Vertical2") * cameraRange.y);
        camera.transform.localPosition = new Vector3(v.x, v.y, -4);
    }

    void StopAttack() {
        attacking = false;

        Vector2 v = rigidbody.velocity;
        v.x = 0;
        rigidbody.velocity = v;
    }

    void Movement() {
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

    void Animations() {
        sprite.transform.localScale = new Vector3(side == RIGHT ? 1 : -1, 1, 1);
        animator.SetBool("Running", running);
        animator.SetBool("Punching", attacking);
        animator.SetBool("Grounded", grounded);
    }
}
