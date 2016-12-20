
using UnityEngine;

public class CooldownTimer {
	public float Cooldown { get; set; }
	public float LastUse { get; set; }
	
	public bool Use {
		get {
			if (Time.time - LastUse > Cooldown) {
				LastUse = Time.time;
				return true;
			} else {
				return false;
			}
		}
	}

    public bool canUse {
        get {
            return Time.time - LastUse > Cooldown;
        }
    }

    public CooldownTimer (float cooldown) {
		Cooldown = cooldown;
		LastUse = Time.time;
	}

	public void Clear() {
		LastUse = Time.time - Cooldown;
	}
}

