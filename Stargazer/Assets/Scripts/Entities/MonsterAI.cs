using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : Entity {

	private const int MAX_HEALTH = 10;
	public GameObject weaponPrefab;
	private Transform centerTransform;

	[SerializeField]
	private GameObject targetRef;

	void Start () {
		centerTransform = transform.GetChild(0);
		this.direction = this.transform.eulerAngles;
		this.position = this.transform.position;

		GetComponent<Health>().SetHealth(MAX_HEALTH);
		Equipment weapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.Euler(0,0,0)).GetComponent<Equipment>();
		GetComponent<EquipAction>().OnEquip(weapon, this.centerTransform);
		
	}

	void Update() {
		//Simplify this.
		print(targetRef);
		if (targetRef != null) {
			Vector3 targetPosition = targetRef.transform.position;
			//If the distance between this object and the player <= 10 units...
			if (Vector3.Distance(centerTransform.position, targetPosition) <= 90) {
				Quaternion q = Quaternion.LookRotation(targetPosition - centerTransform.position);
				centerTransform.rotation = Quaternion.Slerp(centerTransform.rotation, q, 5 * Time.deltaTime);
				Ray ray = new Ray(centerTransform.position, centerTransform.forward);
				RaycastHit hitInfo;
				print("Message");

				if (Physics.Raycast(ray, out hitInfo)) { 
					if (hitInfo.transform.tag == "Player") {
						this.equipment.OnActivate();
					}
				}
			}
		}
	}
	

}
