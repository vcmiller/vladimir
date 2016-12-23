using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Layer {
    public float ymin;
    public float ymax;
    public GameObject[] objects;

    private bool isActive = true;

    public bool active {
        set {
            if (value != isActive) {
                foreach (GameObject obj in objects) {
                    obj.SetActive(value);
                }

                isActive = value;
            }
        }

        get {
            return isActive;
        }
    }
}
