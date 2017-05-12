using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon {
    public ExpirationTimer reloadTimer { get; private set; }
    public float reloadTime;
    public int magSize = 5;

    public int magazine { get; private set; }

    protected override void Start() {
        base.Start();
        reloadTimer = new ExpirationTimer(reloadTime);
        magazine = magSize;
    }

    protected override void Update() {
        if (reloadTimer.Expired && magazine == 0) {
            magazine = magSize;
        }

        if (firing && shootTimer.Use && magazine > 0) {
            Fire();
        }
    }

    protected override void Fire() {
        GameObject proj = Instantiate(projectile);

        proj.transform.position = transform.TransformPoint(fireOffset);
        proj.transform.right = transform.TransformVector(Vector3.right);

        magazine--;

        if (magazine == 0) {
            reloadTimer.Set();
        }
    }
}
