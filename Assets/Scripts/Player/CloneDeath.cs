using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloneDeath : DeathAnimation {
    public GameObject explosionPrefab;
    public RuntimeAnimatorController deathAnimation;

    public override void Play(bool side) {
        Destroy(GetComponent<PlayerClone>());
        GetComponentInChildren<Animator>().runtimeAnimatorController = deathAnimation;
        Destroy(GetComponent<Rigidbody2D>());

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        Invoke("Explode", 0.5f);
    }

    void Explode() {
        if (Controller.inst.currentSave.upgrades[Upgrade.fissionExplode]) {

            GameObject obj = Instantiate(explosionPrefab, transform.position + Vector3.back * 2, Quaternion.identity);
            Material m = obj.GetComponent<MeshRenderer>().material;
            m.SetFloat("_StartTime", Time.timeSinceLevelLoad);
            Destroy(obj, 1);

            foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
                if (Vector3.SqrMagnitude(transform.position - enemy.transform.position) < 9) {
                    enemy.Die(transform.position.x < enemy.transform.position.x);
                }
            }
        }
    }
}
