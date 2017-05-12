using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float fireDelay;
    public GameObject projectile;
    public Vector3 fireOffset;

    public float aim { get; set; }
    public CooldownTimer shootTimer { get; private set; }
    public bool firing { get; set; }

    protected virtual void Start() {
        shootTimer = new CooldownTimer(fireDelay);
    }
    
	protected virtual void Update () {
		if (firing && shootTimer.Use) {
            Fire();
        }
	}

    protected virtual void Fire() {
        GameObject proj = Instantiate(projectile);

        proj.transform.position = transform.TransformPoint(fireOffset);
        proj.transform.right = transform.TransformVector(Vector3.right);
    }
}
