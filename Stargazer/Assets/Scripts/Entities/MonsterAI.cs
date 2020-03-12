using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : Entity {

	private const int MAX_HEALTH = 10;
	public GameObject weaponPrefab;
	private Transform centerTransform;

	[SerializeField]
	private Transform targetRef;
	private bool targetInLineOfSight;
	private bool targetDetected;
	private bool targetInShootingRange;
	private bool targetCanBeSeen;

	private const int DETECTION_RANGE = 80;
	private const int SHOOTING_RANGE = 40;

	public float speed = 20f;

	private MovementAI movementAI;

	void Start () {
		centerTransform = transform.GetChild(0);
		this.direction = this.transform.eulerAngles;
		this.position = this.transform.position;
		this.velocity = Vector3.zero;

		targetDetected = targetCanBeSeen = targetInLineOfSight = targetInShootingRange = false;

		movementAI = GetComponent<MovementAI>();
		GetComponent<Health>().SetHealth(MAX_HEALTH);
		Equipment weapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.Euler(0,0,0)).GetComponent<Equipment>();
		GetComponent<EquipAction>().OnEquip(weapon, this.centerTransform);
	}

	void Update() {
		if (targetRef != null) {
			RaycastHit hitInfo;

			//Sends a raycast to determine if the player can be seen if rotates only.
			Ray rayCanBeSeen = new Ray(centerTransform.position, targetRef.position - centerTransform.position);

			if (Physics.Raycast(rayCanBeSeen, out hitInfo)) { 
				if (hitInfo.transform.tag == "Player") {
					targetCanBeSeen = true;
				} else {
					targetCanBeSeen = false;
				}
			} else {
				targetCanBeSeen = false;
			}

			//Sends a raycast to determine if the player is infront of the enemy.
			Ray rayLineOfSight = new Ray(centerTransform.position, centerTransform.forward);

			if (Physics.Raycast(rayLineOfSight, out hitInfo)) { 
				if (hitInfo.transform.tag == "Player") {
					targetInLineOfSight = true;
				} else {
					targetInLineOfSight = false;
				}
			} else {
				targetInLineOfSight = false;
			}

			//if target is within the spotting range then activate target spotted
			if (Vector3.Distance(targetRef.position, transform.position) >= DETECTION_RANGE) {
				targetDetected = false;
			} else {
				targetDetected = true;
			}

			//if target is within the shooting range then activate target shooting
			if (Vector3.Distance(targetRef.position, transform.position) >= SHOOTING_RANGE) {
				targetInShootingRange = false;
			} else {
				targetInShootingRange = true;
			}
		}

		//If target is spotted...
		if (targetDetected) {
			if (!targetInShootingRange || !targetCanBeSeen) {
				movementAI.NavigateTo(targetRef.position);
			} else {
				movementAI.EndNavigation();
			}

			if (targetInLineOfSight) {
				this.equipment.OnActivate();
			} else if (targetCanBeSeen) {
				Quaternion q = Quaternion.LookRotation(targetRef.position - centerTransform.position);
				centerTransform.rotation = Quaternion.Slerp(centerTransform.rotation, q, 5 * Time.deltaTime);
			}
		}
	}

	private void FixedUpdate() {	
		if (this.velocity != Vector3.zero) {
			transform.Translate(this.velocity * this.speed * Time.fixedDeltaTime);
			this.position = transform.position;
			transform.eulerAngles = this.direction;
		}
	}

}
