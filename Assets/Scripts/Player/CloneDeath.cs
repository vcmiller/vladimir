using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloneDeath : DeathAnimation {
    public GameObject explosionPrefab;
    public RuntimeAnimatorController deathAnimation;

    public override void Play(bool side) {
        Destroy(GetComponent<Player>());
        GetComponentInChildren<Animator>().runtimeAnimatorController = deathAnimation;
        Destroy(GetComponent<Rigidbody2D>());

        foreach (Ability abil in GetComponentsInChildren<Ability>()) {
            Destroy(abil);
        }

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        Invoke("Explode", 0.5f);
    }

    void Explode() {
        GameObject obj = Instantiate(explosionPrefab, transform.position + Vector3.back * 2, Quaternion.identity);
        Material m = obj.GetComponent<MeshRenderer>().material;

        Destroy(obj, 0.5f);
        m.SetFloat("_StartTime", Time.timeSinceLevelLoad);
    }
}
