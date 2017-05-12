using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {
    public float deviation;
    public int pellets;

    protected override void Fire() {
        for (int i = 0; i < pellets; i++) {
            GameObject proj = Instantiate(projectile);

            proj.transform.position = transform.TransformPoint(fireOffset);
            proj.transform.right = transform.TransformVector(Vector3.right);
            proj.transform.Rotate(0, 0, Random.Range(-deviation, deviation), Space.World);
        }
    }
}
