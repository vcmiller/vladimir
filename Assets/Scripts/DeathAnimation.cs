using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeathAnimation : MonoBehaviour {
    protected const bool RIGHT = true;
    protected const bool LEFT = false;

    public abstract void Play(bool side);
}
