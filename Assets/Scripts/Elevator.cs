using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {
    public Vector3 targetPosition;
    public Vector3 startPosition { get; private set; }
    public bool playerInside { get; private set; }
    public Rigidbody2D rigidbody { get; private set; }
    public float moveSpeed = 1;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>()) {
            playerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<Player>()) {
            playerInside = false;
        }
    }

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 target = playerInside ? startPosition + targetPosition : startPosition;
        rigidbody.MovePosition(Vector2.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime));
        
	}
}
