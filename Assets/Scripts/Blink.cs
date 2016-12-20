using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour {
    public float amplitude;
    public float period;

    public SpriteRenderer sprite { get; private set; }

    // Use this for initialization
    void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        Color c = sprite.color;
        c.a = 1.0f - (0.5f + 0.5f * Mathf.Sin(Time.time * 2 * Mathf.PI / period)) * amplitude;
        sprite.color = c;
    }
}
