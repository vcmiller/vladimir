using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : Ability {
    public float duration;

    public ExpirationTimer expiration { get; private set; }

    public override void Awake() {
        base.Awake();

        expiration = new ExpirationTimer(duration);
    }

    void Update() {
        if (Input.GetButtonDown(button) && useTimer.Use) {
            expiration.Set();
        }

        player.slowTime = !expiration.Expired;
    }
}
