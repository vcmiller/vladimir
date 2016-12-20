using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplodeDeath : DeathAnimation {
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
        Invoke("EndGame", 2);
    }

    void Explode() {
        GameObject obj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Material m = obj.GetComponent<MeshRenderer>().material;

        foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
            enemy.Die(enemy.transform.position.x > transform.position.x);
        }

        m.SetFloat("_StartTime", Time.timeSinceLevelLoad);
    }

    void EndGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
