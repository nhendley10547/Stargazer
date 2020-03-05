using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour {

	private const int MAX_HEALTH = 10;

	void Start () {
		GetComponent<Health>().SetHealth(MAX_HEALTH);
		GameObject revolverPrefab = (GameObject)Resources.Load("Prefabs/Revolver", typeof(GameObject));
		Equipment revolver = Instantiate(revolverPrefab, Vector3.zero, Quaternion.Euler(0,0,0)).GetComponent<Equipment>();
		GetComponent<EquipAction>().OnEquip(revolver, this.transform);
	}
}
