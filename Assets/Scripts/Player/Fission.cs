using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fission : Ability {
    public float duration;
    public GameObject clonePrefab;
    public Vector3 clonePosition;
    public float speed;
    public float reticleDist;

    public Vector2 aim { get; private set; }

    public PlayerClone clone { get; private set; }

    public override void Awake() {
        base.Awake();
        aim = new Vector2(1, 0);
    }

    public override void Update() {
        base.Update();

        if (Input.GetButtonDown(button) && useTimer.canUse && !clone && !player.targetting) {
            player.targetting = true;
        }
        
        if (Input.GetButtonUp(button) && !clone && player.targetting && useTimer.Use) {
            clone = Instantiate(clonePrefab, transform.position + (Vector3)aim.normalized, Quaternion.identity).GetComponent<PlayerClone>();

            clone.Invoke("Die", duration);
            clone.side = player.side;
            clone.rigidbody.velocity = aim.normalized * speed;

            player.targetting = false;
        }

        if (Input.GetButton(button) && player.targetting) {
            Vector3 v = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (v.magnitude > 0.8f) {
                aim = v;
                player.reticle.transform.localPosition = (Vector3)aim.normalized * reticleDist + Vector3.back * 2;
                player.side = aim.x > 0;
            }
        }
    }
}
