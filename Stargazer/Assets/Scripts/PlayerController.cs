using UnityEngine;

public class PlayerController : Entity {

	public Transform playerView;
	private Collider playerCollider;
	private Rigidbody playerBody;
	private Equipment equipment;

	private float speed = 5.0f;
	private float jumpHeight = 2.0f;
	private float mouseSensitivity = 200.0f;
	private float yRotation = 0.0f;

	new void Start() {
		base.Start();
		playerView.parent = transform;
		playerView.position = transform.position + Vector3.up * .5f;
		playerView.eulerAngles = this.direction = transform.eulerAngles;
		playerBody = GetComponent<Rigidbody>();
		playerCollider = GetComponent<Collider>();
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update() {
		this.MoveControl();
		this.JumpControl();
		this.RotatePerspective();
		this.InteractionControl();
	}

	void FixedUpdate() {
		playerBody.MovePosition(playerBody.position + this.velocity * Time.deltaTime);
		position = playerView.transform.position;
	}

	void MoveControl() {
		Vector3 input = new Vector3(-Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
		if (input != Vector3.zero) {
			float inputDirection = Mathf.Atan2(input.normalized.z, input.normalized.x);
			float facingDirection = Mathf.Deg2Rad * transform.eulerAngles.y;
			float moveDirection = inputDirection - (Mathf.PI / 2) + facingDirection;
			Vector3 direction = new Vector3(Mathf.Sin(moveDirection), 0.0f, Mathf.Cos(moveDirection));
			this.velocity = direction * speed;
		} else {
			this.velocity = Vector3.zero;
		}
	}

	void JumpControl() {
		bool isGrounded = Physics.CheckSphere(transform.position - playerCollider.bounds.extents.y * Vector3.up, 0.2f, this.groundLayer, QueryTriggerInteraction.Ignore);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerBody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
	}

	void InteractionControl() {
		Ray ray = new Ray(playerView.position, playerView.forward);
		RaycastHit hitInfo;

		if (Input.GetMouseButtonDown(1)) { 
			if (Physics.Raycast(ray, out hitInfo, 5, ~this.itemLayer)) {
				Debug.Log("Parent: " + hitInfo.transform.tag);
				if (hitInfo.transform.tag == "Equipment" ) {

					if (this.equipment != null) {
						this.equipment.OnDrop();
						this.equipment = null;
					}

					this.equipment = hitInfo.transform.gameObject.GetComponent<Equipment>();;
					this.equipment.OnEquip(this, playerView);
				}
			}
		}

		if (Input.GetMouseButton(0) && this.equipment != null) {
			this.equipment.OnActivate();
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			if (this.equipment != null) {
				this.equipment.OnDrop();
				this.equipment = null;
			}
		} 
	}

	void RotatePerspective() {
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		yRotation = Mathf.Clamp(yRotation - mouseY, -90f, 60f);

		playerView.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
		transform.Rotate(Vector3.up * mouseX);	

		this.direction = playerView.eulerAngles;
	}


}
