using UnityEngine;

public class Revolver : Equipment {

	public GameObject bulletPrefab;
	private float currentReloadTime = 0;
    private const float MAX_RELOAD_TIME = .6f;
    private const float BULLET_SPEED = 12.0f;
    private const float BULLET_RANGE = 100.0f;

    public override void OnActivate() {
        if (this.currentReloadTime <= 0) {
            Vector3 bulletDirection = Calculate.HeadingBasedDirection(this.transform.position, this.transform.eulerAngles);
            Vector3 position = this.transform.GetChild(0).position + bulletDirection;

            GameObject bulletClone = Instantiate(bulletPrefab, position, this.transform.rotation) as GameObject;
            bulletClone.GetComponent<Bullet>().Init(bulletDirection, BULLET_SPEED, BULLET_RANGE);

            this.currentReloadTime = MAX_RELOAD_TIME;
        }
    }

    void Update() {
		if (!(this.currentReloadTime <= 0)) {
			this.currentReloadTime -= Time.deltaTime;
		}
    }
}