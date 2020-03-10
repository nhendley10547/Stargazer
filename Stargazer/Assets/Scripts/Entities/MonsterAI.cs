using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : Entity {

	private const int MAX_HEALTH = 10;
	public GameObject weaponPrefab;
	private Transform centerTransform;
	private Rigidbody body;

	[SerializeField]
	private Transform targetRef;

	public float speed = 5f;
	public float turnSpeed = 1;
	public float turnDst = 5;
	const float pathUpdateMoveThreshold = .5f;
	const float minPathUpdateTime = .2f;
	public float stoppingDst = 50;

	Path path;

	void Start () {
		StartCoroutine(UpdatePath());

		centerTransform = transform.GetChild(0);
		this.direction = this.transform.eulerAngles;
		this.position = this.transform.position;

		body = GetComponent<Rigidbody>();
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

	public void OnPathFound(Vector3[] waypoints, bool success) {
		if (success) {
			this.path = new Path(waypoints, transform.position, turnDst, stoppingDst);
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator UpdatePath() {
		
		if (Time.timeSinceLevelLoad < .3f) {
			yield return new WaitForSeconds(.3f);
		}

		PathRequestManager.RequestPath(new PathRequest(transform.position, targetRef.position, OnPathFound));
		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		Vector3 targetPosOld = targetRef.position;

		while(true) {
			yield return new WaitForSeconds(minPathUpdateTime);
			if((targetRef.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
				PathRequestManager.RequestPath(new PathRequest(transform.position, targetRef.position, OnPathFound));
				targetPosOld = targetRef.position;
			}
		}
	}
	
	IEnumerator FollowPath() {
		bool followingPath = true;
		int pathIndex = 0;
		transform.LookAt(path.lookPoints[0]);

		float speedPercent = 1;

		while (followingPath) {
			Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
			while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D)) {
				if (pathIndex == path.finishLineIndex) {
					followingPath = false;
					break;
				} else {
					pathIndex++;
				}
			}

			if (followingPath) {
				if (pathIndex >= path.slowDownIndex && stoppingDst > 0) {
					speedPercent = path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst;
					if (speedPercent < 0.01f) {
						followingPath = false;
					}
				}
				Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
				Quaternion rot = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
				Vector3 dir = rot.eulerAngles * Mathf.PI;
				dir.Normalize();

				print(dir);
				body.MoveRotation(rot);
				//transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
				body.MovePosition(transform.position + Vector3.forward * Time.deltaTime * speed * speedPercent);
				
			}


			yield return null;
		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			path.DrawWithGizmos();
		}
	}
}
