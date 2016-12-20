using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClone : Player {
    protected override void Awake() {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        health = maxHealth;
    }

    // Update is called once per frame
    protected override void Update() {
        UpdateGrounded();
        Attacks();

        Animations();
    }

    protected override void FixedUpdate() {
        Movement();
    }
}
