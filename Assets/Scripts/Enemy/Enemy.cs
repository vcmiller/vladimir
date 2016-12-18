using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float moveSpeed = 3;
    public float viewDist = 8;
    public float viewDot = 0.5f;

    public Transform sprite { get; private set; }
    public StateMachine machine { get; private set; }
    public Transform target { get; private set; }
    public Transform gun { get; private set; }
    public Animator animator { get; private set; }

    public bool side { get; private set; }
    public bool walking { get; private set; }

    const bool RIGHT = true;
    const bool LEFT = false;

	// Use this for initialization
	void Start () {
        sprite = GetComponentInChildren<SpriteRenderer>().transform;
        machine = GetComponent<StateMachine>();
        gun = sprite.GetChild(0);
        animator = GetComponentInChildren<Animator>();

        State patrol = new State(Patrol);
        State attack = new State(Attack);
        Transition patrolToAttack = new Transition(patrol, attack, PlayerInView);

        machine.AddState(patrol);
        machine.AddState(attack);

        machine.AddTransition(patrolToAttack);
	}

    bool PlayerInView() {
        if (FindTarget()) {
            Vector3 forward;
            
            if (side == RIGHT) {
                forward = Vector3.right;
            } else {
                forward = Vector3.left;
            }

            Vector3 toPlayer = target.position - transform.position;
            Vector3 toPlayerDir = toPlayer.normalized;
            
            if (Vector3.Dot(forward, toPlayerDir) > viewDot) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer);
                if (!hit.collider || hit.distance > toPlayer.magnitude) {
                    return true;
                }
            }
        }

        return false;
    }
	
    void Attack() {
        if (FindTarget()) {
            FaceTarget();
            UpdateSprite();
            Aim();

            walking = false;

            if (!Avoidance() && Mathf.Abs(transform.position.x - target.position.x) > 2.0f) {
                Movement();
            }
        }
    }

    void Aim() {
        Vector2 v = target.transform.position - gun.transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(v.y, side ? v.x : -v.x);
        gun.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    void FaceTarget() {
        if (target.transform.position.x < transform.position.x) {
            side = LEFT;
        } else {
            side = RIGHT;
        }
    }

    bool FindTarget() {
        if (!target) {
            target = FindObjectOfType<Player>().transform;
        }

        return target;
    }

    void Patrol() {
        if (Avoidance()) {
            side = !side;
        }

        Movement();
        UpdateSprite();
    }

    void UpdateSprite() {
        if (side == RIGHT) {
            sprite.transform.localScale = new Vector3(1, 1, 1);
        } else {
            sprite.transform.localScale = new Vector3(-1, 1, 1);
        }

        animator.SetBool("Walking", walking);

        gun.transform.localEulerAngles = Vector3.zero;
    }

    void Movement() {
        if (side == RIGHT) {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        } else {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        walking = true;
    }

    bool Avoidance() {
        RaycastHit2D hit;
        if (side == RIGHT) {
            hit = Physics2D.CapsuleCast(transform.position, new Vector2(0.5f, 1.0f), CapsuleDirection2D.Vertical, 0, Vector3.right, 0.5f);
        } else {
            hit = Physics2D.CapsuleCast(transform.position, new Vector2(0.5f, 1.0f), CapsuleDirection2D.Vertical, 0, Vector3.left, 0.5f);
        }

        if (hit.collider) {
            return true;
        } else {
            if (side == RIGHT) {
                hit = Physics2D.CapsuleCast(transform.position + Vector3.right * 0.5f, new Vector2(0.5f, 1.0f), CapsuleDirection2D.Vertical, 0, Vector3.down, 1.0f);
            } else {
                hit = Physics2D.CapsuleCast(transform.position + Vector3.left * 0.5f, new Vector2(0.5f, 1.0f), CapsuleDirection2D.Vertical, 0, Vector3.down, 1.0f);
            }

            if (!hit.collider) {
                return true;
            }
        }

        return false;
    }
}
