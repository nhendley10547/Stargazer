using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounterUI : Entity {

	public void Ammo () {
		if (this.equipment != null) {
			Text txtAmmo = GameObject.Find("UI/AmmoCounter").GetComponent<Text>();
			txtAmmo.text = "Ammo: " + this.equipment.GetAmmoCount();
		}
	}
}
