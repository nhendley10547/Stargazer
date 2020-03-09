using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : Entity {

	private const int MAX_HEALTH = 10;
	public GameObject weaponPrefab;
	private Transform centerTransform;

	[SerializeField]
	private Transform targetRef;
	float speed = 5f;
	Vector3[] path;
	int targetIndex;

	void Start () {
		PathRequestManager.RequestPath(transform.position, targetRef.position, OnPathFound);

		centerTransform = transform.GetChild(0);
		this.direction = this.transform.eulerAngles;
		this.position = this.transform.position;

		GetComponent<Health>().SetHealth(MAX_HEALTH);
		Equipment weapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.Euler(0,0,0)).GetComponent<Equipment>();
		GetComponent<EquipAction>().OnEquip(weapon, this.centerTransform);
		
	}

	void Update() {
		//Simplify this.
		// if (targetRef != null) {
		// 	Vector3 targetPosition = targetRef.position;
		// 	//If the distance between this object and the player <= 10 units...
		// 	if (Vector3.Distance(centerTransform.position, targetPosition) <= 90) {
		// 		Quaternion q = Quaternion.LookRotation(targetPosition - centerTransform.position);
		// 		centerTransform.rotation = Quaternion.Slerp(centerTransform.rotation, q, 5 * Time.deltaTime);
		// 		Ray ray = new Ray(centerTransform.position, centerTransform.forward);
		// 		RaycastHit hitInfo;

		// 		if (Physics.Raycast(ray, out hitInfo)) { 
		// 			if (hitInfo.transform.tag == "Player") {
		// 				this.equipment.OnActivate();
		// 			}
		// 		}
		// 	}
		// }
	}

	public void OnPathFound(Vector3[] newPath, bool success) {
		if (success) {
			this.path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}
	
	IEnumerator FollowPath() {
		Vector3 currentWaypoint = path[0];

		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex++;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			currentWaypoint.y = transform.position.y;
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, this.speed * Time.deltaTime);
			yield return null;
		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);
				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				} else {
					Gizmos.DrawLine(path[i-1], path[i]);
				}
			}
		}
	}
}
