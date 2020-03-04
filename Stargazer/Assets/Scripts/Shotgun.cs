using UnityEngine;

public class Shotgun : Equipment {

    public GameObject bulletPrefab;
    private float currentReloadTime = 0;
    private const float MAX_RELOAD_TIME = .6f;
    private const float BULLET_SPEED = 12.0f;
    private const float BULLET_RANGE = 100.0f;
    Vector3[] position = new Vector3[6];
    GameObject[] bulletClone = new GameObject[6];
    
    public override void OnActivate() {
        int[] rnd = new int[6];
        int[] rnd2 = new int[6];

        if (this.currentReloadTime <= 0) {
            for (int i = 0; i < 6; i++)
            {
                rnd[i] = Random.Range(-5, 5);
                rnd2[i] = Random.Range(-5, 5);
                Vector3 bulletDirection = Calculate.HeadingBasedDirection(this.transform.position, this.transform.eulerAngles + new Vector3(rnd[i], rnd2[i], 0));
                position[i] = this.transform.GetChild(0).position + bulletDirection * (.1f * i);
                bulletClone[i] = Instantiate(bulletPrefab, position[i], this.transform.rotation) as GameObject;
                bulletClone[i].transform.localScale = new Vector3(.05f, .05f, .05f);
                bulletClone[i].GetComponent<Bullet>().Init(bulletDirection, BULLET_SPEED, BULLET_RANGE);
            }

            this.currentReloadTime = MAX_RELOAD_TIME;
        }
    }

    public override void OnEquip(Entity owner, Transform viewTransform) {
        base.OnEquip(owner, viewTransform);

        Rigidbody body = GetComponent<Rigidbody>();
        body.isKinematic = true;
        body.useGravity = false;
        body.detectCollisions = false;

        this.transform.position = Calculate.DirectionBasedPosition(this.owner.position, this.owner.direction + Vector3.right * 20, 1.0f);

        this.transform.eulerAngles = this.owner.direction;
    }

    public override void OnDrop() {
        Vector3 upDirection = new Vector3(0, this.owner.direction.y, 0);
        this.transform.position = Calculate.DirectionBasedPosition(this.owner.position, upDirection, 1.0f);

        Rigidbody body = GetComponent<Rigidbody>();
        body.isKinematic = false;
        body.useGravity = true;
        body.detectCollisions = true;

        base.OnDrop();
    }

    void Update() {
        if (!(this.currentReloadTime <= 0)) {
            this.currentReloadTime -= Time.deltaTime;
        }
    }
}