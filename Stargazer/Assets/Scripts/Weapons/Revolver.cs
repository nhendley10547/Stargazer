using UnityEngine;
using UnityEngine.UI;

public class Revolver : Equipment {

	public GameObject bulletPrefab;
	private float currentReloadTime = 0;
    private const float MAX_RELOAD_TIME = .6f;
    private const float BULLET_SPEED = 12.0f;
    private const float BULLET_RANGE = 100.0f;

    void Start() {
        this.maxAmmoCount = 20;
        this.currentAmmoCount = this.maxAmmoCount;
        this.id = "Revolver";
    }

    public override void OnActivate() {
        if (this.currentAmmoCount > 0) {
            if (this.currentReloadTime <= 0) {

                Vector3 bulletDirection = Calculate.DirectionFromAngle(this.transform.eulerAngles);
                Vector3 position = this.transform.GetChild(0).position + bulletDirection * .2f;

                GameObject bulletClone = Instantiate(bulletPrefab, position, this.transform.rotation) as GameObject;
                bulletClone.GetComponent<Bullet>().Init(bulletDirection, BULLET_SPEED, BULLET_RANGE);

                this.currentReloadTime = MAX_RELOAD_TIME;
                this.currentAmmoCount--;
            }
        }
    }

    void Update() {
		if (!(this.currentReloadTime <= 0)) {
			this.currentReloadTime -= Time.deltaTime;
		}
    }
}