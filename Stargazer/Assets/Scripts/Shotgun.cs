using UnityEngine;

public class Shotgun : Equipment {

    public GameObject bulletPrefab;
    private float currentReloadTime = 0;
    private const float MAX_RELOAD_TIME = .6f;
    private const float BULLET_SPEED = 12.0f;
    private const float BULLET_RANGE = 100.0f;

    public override void OnActivate() {
        if (this.currentReloadTime <= 0) {
            Vector3 bulletDirection = Calculate.HeadingBasedDirection(this.transform.position, this.transform.eulerAngles);
            Vector3 position = this.transform.GetChild(0).position + bulletDirection * .1f;
            Vector3 position1 = this.transform.GetChild(0).position + bulletDirection * .2f;
            Vector3 position2 = this.transform.GetChild(0).position + bulletDirection * .3f;
            Vector3 position3 = this.transform.GetChild(0).position + bulletDirection * .4f;
            Vector3 position4 = this.transform.GetChild(0).position + bulletDirection * .5f;

            GameObject bulletClone = Instantiate(bulletPrefab, position, this.transform.rotation) as GameObject;
            GameObject bulletClone1 = Instantiate(bulletPrefab, position1, this.transform.rotation) as GameObject;
            GameObject bulletClone2 = Instantiate(bulletPrefab, position2, this.transform.rotation) as GameObject;
            GameObject bulletClone3 = Instantiate(bulletPrefab, position3, this.transform.rotation) as GameObject;
            GameObject bulletClone4 = Instantiate(bulletPrefab, position4, this.transform.rotation) as GameObject;
            bulletClone.transform.localScale = new Vector3(.05f, .05f, .05f);
            bulletClone1.transform.localScale = new Vector3(.05f, .05f, .05f);
            bulletClone2.transform.localScale = new Vector3(.05f, .05f, .05f);
            bulletClone3.transform.localScale = new Vector3(.05f, .05f, .05f);
            bulletClone4.transform.localScale = new Vector3(.05f, .05f, .05f);
            bulletClone.GetComponent<Bullet>().Init(bulletDirection, BULLET_SPEED, BULLET_RANGE);
            bulletClone1.GetComponent<Bullet>().Init(bulletDirection, BULLET_SPEED, BULLET_RANGE);
            bulletClone2.GetComponent<Bullet>().Init(bulletDirection, BULLET_SPEED, BULLET_RANGE);
            bulletClone3.GetComponent<Bullet>().Init(bulletDirection, BULLET_SPEED, BULLET_RANGE);
            bulletClone4.GetComponent<Bullet>().Init(bulletDirection, BULLET_SPEED, BULLET_RANGE);

            this.currentReloadTime = MAX_RELOAD_TIME;
        }
    }

    void Update() {
        if (!(this.currentReloadTime <= 0)) {
            this.currentReloadTime -= Time.deltaTime;
        }
    }
}