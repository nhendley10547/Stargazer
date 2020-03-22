using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public int entityHealth = 10;

	public void SetHealth(int health) {
		this.entityHealth = health;
	}

	public void ChangeHealthBy(int healthValue) {
		this.entityHealth -= healthValue;
		if (this.entityHealth <= 0) {
			this.gameObject.GetComponent<Entity>().Death();
		}
	}
}
