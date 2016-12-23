using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour {
    public float cooldown;
    public CooldownTimer useTimer { get; private set; }
    public Player player { get; private set; }
    public string button;

    public virtual void Awake() {
        useTimer = new CooldownTimer(cooldown);
        useTimer.Clear();
        player = GetComponent<Player>();
    }
	
}
