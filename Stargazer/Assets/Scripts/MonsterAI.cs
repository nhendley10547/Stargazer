using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : Entity {

	private const int MAX_HEALTH = 10;
	public GameObject revolverPrefab;
	private Transform centerTransform;

	[SerializeField]
	private GameObject targetRef;

	void Start () {
		centerTransform = transform.GetChild(0);
		this.direction = this.transform.eulerAngles;
		this.position = this.transform.position;

		GetComponent<Health>().SetHealth(MAX_HEALTH);
		Equipment revolver = Instantiate(revolverPrefab, Vector3.zero, Quaternion.Euler(0,0,0)).GetComponent<Equipment>();
		GetComponent<EquipAction>().OnEquip(revolver, this.centerTransform);
	}

	void Update() {
		//If the distance between this object and the player <= 4 units...
		if (Vector3.Distance(centerTransform.position, targetRef.transform.position) <= 4) {
			//Rotate the transform to look at the player
			Vector3 targetAngle = Calculate.FacingPositionAngle(centerTransform.position, targetRef.transform.position);
			centerTransform.eulerAngles = Vector3.RotateTowards(centerTransform.eulerAngles, targetAngle, 2 * Mathf.PI, 1);
		}
	}
	

}
