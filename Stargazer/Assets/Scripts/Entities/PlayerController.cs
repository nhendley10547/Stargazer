using UnityEngine;

public class PlayerController : Entity {

	public Camera playerView;
	private Collider playerCollider;
	private Rigidbody playerBody;

    private float speed = 5.0f;
	private float jumpHeight = 2.0f;
	private float mouseSensitivity = 200.0f;
	private float yRotation = 0.0f;

	private EquipAction equipAction;
	public GameObject weaponPrefab;

	void Start() {
		playerView.enabled = true;
		playerView.transform.position = transform.position + Vector3.up * .5f;
		playerView.transform.eulerAngles = this.direction = transform.eulerAngles;
		playerBody = GetComponent<Rigidbody>();
		playerCollider = GetComponent<Collider>();
        equipAction = GetComponent<EquipAction>();

        if (weaponPrefab != null)
        {
            Equipment weapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<Equipment>();
            equipAction.OnEquip(weapon, playerView.transform);
        }
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
		
		if (Input.GetKeyDown(KeyCode.E)) { 
			Ray ray = new Ray(playerView.transform.position, playerView.transform.forward);
			RaycastHit hitInfo;

			if (Physics.Raycast(ray, out hitInfo, 5, LayerMask.NameToLayer("Item"))) {
				if (hitInfo.transform.tag == "Equipment" ) {
					Equipment item = hitInfo.transform.GetComponent<Equipment>();
					if (this.equipment != null) {
						this.equipAction.OnDrop(this.equipment);
					}

					print("Name: " + hitInfo.transform.name);

					this.equipAction.OnEquip(item, playerView.transform);
				}
			} else if (!Physics.Raycast(ray, out hitInfo, 3) && this.equipment != null) {
				this.equipAction.OnDrop(this.equipment);
			}
		}

		if (Input.GetMouseButton(0) && this.equipment != null) {
			this.equipment.OnActivate();
		}
	}

	void RotatePerspective() {
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		yRotation = Mathf.Clamp(yRotation - mouseY, -90f, 60f);

		playerView.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
		transform.Rotate(Vector3.up * mouseX);	

		this.direction = playerView.transform.eulerAngles;
	}

	public override void Death() {
		playerView.transform.parent = null;
		print("YOU DIED!");
		Destroy(this.gameObject);
	}
}
