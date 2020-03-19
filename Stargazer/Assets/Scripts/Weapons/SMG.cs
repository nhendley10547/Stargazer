using UnityEngine;
using UnityEngine.UI;

public class SMG : Equipment {

    public GameObject bulletPrefab;
    private float currentReloadTime = 0;
    private const float MAX_RELOAD_TIME = .025f;
    private const float BULLET_SPEED = 20.0f;
    private const float BULLET_RANGE = 100.0f;
    private const float MAX_AMMO = 100;
    private float currentAmmo = 99;


    public override void OnActivate() {
        if (currentAmmo >= 0) {
            int RND = Random.Range(-1, 1);
            int RND2 = Random.Range(-2, 2);

            if (this.currentReloadTime <= 0) {
                Text txtAmmo = GameObject.Find("UI/AmmoCounter").GetComponent<Text>();
                txtAmmo.text = "Ammo: " + currentAmmo + "/" + MAX_AMMO;

                Vector3 bulletDirection = Calculate.DirectionFromAngle(this.transform.eulerAngles + new Vector3(RND, RND2, 0));
                Vector3 position = this.transform.GetChild(0).position + bulletDirection * .2f;

                GameObject bulletClone = Instantiate(bulletPrefab, position, this.transform.rotation) as GameObject;
                bulletClone.transform.localScale = new Vector3(0.05f, .05f, .05f);
                bulletClone.GetComponent<LasBolt>().Init(bulletDirection, BULLET_SPEED, BULLET_RANGE);

                this.currentReloadTime = MAX_RELOAD_TIME;
                currentAmmo--;
            }
        }
    }


    void Update() {
        if (!(this.currentReloadTime <= 0)) {
            this.currentReloadTime -= Time.deltaTime;
        }
    }
}