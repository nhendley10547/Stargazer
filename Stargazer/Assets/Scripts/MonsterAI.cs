using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour {

	private const int MAX_HEALTH = 10;

	void Start () {
		GetComponent<Health>().SetHealth(MAX_HEALTH);
	}
}
