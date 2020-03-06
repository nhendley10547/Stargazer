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
		//Simplify this.
		if (targetRef != null) {
			Vector3 targetPosition = targetRef.transform.position;
			//If the distance between this object and the player <= 10 units...
			if (Vector3.Distance(centerTransform.position, targetPosition) <= 10) {
				Quaternion q = Quaternion.LookRotation(targetPosition - centerTransform.position);
				centerTransform.rotation = Quaternion.Slerp(centerTransform.rotation, q, 5 * Time.deltaTime);
				this.equipment.OnActivate();
			}
		}
	}
	

}
