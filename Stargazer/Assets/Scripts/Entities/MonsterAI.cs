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
		centerTransform.rotation = transform.rotation;
		this.direction = this.transform.eulerAngles;
		this.position = this.transform.position;
		this.velocity = Vector3.zero;

		targetDetected = targetCanBeSeen = targetInLineOfSight = targetInShootingRange = false;

		movementAI = GetComponent<MovementAI>();
		GetComponent<Health>().SetHealth(MAX_HEALTH);
		Equipment weapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.Euler(0,0,0)).GetComponent<Equipment>();
		GetComponent<EquipAction>().OnEquip(weapon, this.centerTransform);
	}

	void CheckStatus() {
		RaycastHit hitInfo;

		//Sends a raycast to determine if the player can be seen.
		Vector3 dir =  targetRef.position - centerTransform.position;
		
		float x;
		float z;
		float radians = Mathf.Atan2(dir.z, dir.x);

		RaycastHit hitRayLeft;
		// Quaternion spreadAngleLeft = Quaternion.AngleAxis(-1, Vector3.up);
		// Vector3 angleLeft = spreadAngleLeft * dir;
		x = .7f * Mathf.Cos(radians + Mathf.PI / 2);
		z = .7f * Mathf.Sin(radians + Mathf.PI / 2);
		Ray raySpreadLeft = new Ray (centerTransform.position + new Vector3(x, 0, z), dir);
		bool left = Physics.Raycast(raySpreadLeft, out hitRayLeft);
		Debug.DrawLine(centerTransform.position + new Vector3(x, 0, z), hitRayLeft.point, Color.blue);

		RaycastHit hitRayRight;
		// Quaternion spreadAngleRight = Quaternion.AngleAxis(1, Vector3.up);
		// Vector3 angleRight = spreadAngleRight * dir;
		x = .7f * Mathf.Cos(radians - Mathf.PI / 2);
		z = .7f * Mathf.Sin(radians - Mathf.PI / 2);
		Ray raySpreadRight = new Ray (centerTransform.position + new Vector3(x, 0, z), dir);
		bool right = Physics.Raycast(raySpreadRight, out hitRayRight);
		Debug.DrawLine(centerTransform.position + new Vector3(x, 0, z), hitRayRight.point, Color.blue);

		Ray rayCanBeSeen = new Ray(centerTransform.position, dir);
		bool center = Physics.Raycast(rayCanBeSeen, out hitInfo);
		Debug.DrawLine(centerTransform.position, hitInfo.point, Color.green);

		if (left && right && center) {
			if (hitInfo.transform.tag == "Player" && 
				hitRayLeft.transform.tag == "Player" && 
				hitRayRight.transform.tag == "Player") {
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

		if (Vector3.Distance(targetRef.position, transform.position) >= SHOOTING_RANGE) {
			targetInShootingRange = false;
		} else {
			targetInShootingRange = true;
		}
	}

	void AIBehavior() {
		//If target is spotted...
		if (targetDetected) {
			if (movementAI != null) {
				if (!targetInShootingRange || !targetCanBeSeen) {
					movementAI.NavigateTo(targetRef.position);
				} else {
					movementAI.EndNavigation();
				}

				if (targetInLineOfSight) {
					this.equipment.OnActivate();
				}
			}

			if (targetCanBeSeen && targetInShootingRange) {
				Quaternion q = Quaternion.LookRotation(targetRef.position - centerTransform.position);

				centerTransform.rotation = Quaternion.Slerp(centerTransform.rotation, q, 5 * Time.deltaTime);
			}
		}
	}

	void Update() {
		if (targetRef != null) {
			CheckStatus();
			AIBehavior();
		}
	}

	private void FixedUpdate() {	
		if (this.velocity != Vector3.zero) {
			transform.Translate(this.velocity * this.speed * Time.fixedDeltaTime);
			this.position = transform.position;
			transform.eulerAngles = this.direction;
			centerTransform.rotation = Quaternion.Slerp(centerTransform.rotation, transform.rotation, 5*Time.deltaTime);
		}
	}

}
