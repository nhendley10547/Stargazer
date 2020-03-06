using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	int entityHealth;
	
	void Start() {
		entityHealth = 10; // Default health value
	}

	public void SetHealth(int health) {
		this.entityHealth = health;
	}

	public void ChangeHealthBy(int healthValue) {
		this.entityHealth -= healthValue;
		print("Hit! Damage: " + healthValue);
	}

	void Update () {
		if (this.entityHealth <= 0) {
			this.gameObject.GetComponent<Entity>().Death();
		}
	}
}
