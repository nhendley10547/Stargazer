using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : Entity {

	private const int MAX_HEALTH = 10;
	public GameObject revolverPrefab;

	void Start () {
		this.direction = this.transform.eulerAngles;
		this.position = this.transform.position;

		GetComponent<Health>().SetHealth(MAX_HEALTH);
		Equipment revolver = Instantiate(revolverPrefab, Vector3.zero, Quaternion.Euler(0,0,0)).GetComponent<Equipment>();
		GetComponent<EquipAction>().OnEquip(revolver, this.transform);
	}
}
