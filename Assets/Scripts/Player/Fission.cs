using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fission : Ability {
    public float duration;
    public GameObject clonePrefab;
    public Vector3 clonePosition;

    public GameObject clone { get; private set; }

    public override void Awake() {
        base.Awake();
    }

    void Update() {
        if (Input.GetButtonDown(button) && useTimer.Use && !clone) {
            clone = Instantiate(clonePrefab, transform.position + clonePosition, Quaternion.identity);
            Destroy(clone, duration);
        }
    }
}
