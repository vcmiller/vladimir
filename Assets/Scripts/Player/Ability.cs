using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour {
    public float cooldown;
    public CooldownTimer useTimer { get; private set; }
    public Player player { get; private set; }
    public string button;

    public UpgradableValue<float> actualCooldown;

    public virtual void Awake() {
        useTimer = new CooldownTimer(cooldown);
        useTimer.Clear();
        player = GetComponent<Player>();

        actualCooldown = new UpgradableValue<float>(cooldown, cooldown * 0.7f, Upgrade.generalCooldowns);
    }

    public virtual void Update() {
        useTimer.Cooldown = actualCooldown;
    }
	
}
