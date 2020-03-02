using UnityEngine;

public abstract class Equipment : MonoBehaviour {
    
    protected Entity owner;

    public abstract void OnActivate();

    public virtual void OnEquip(Entity owner, Transform viewTransform) {
        GetComponent<BoxCollider>().enabled = false;
        this.owner = owner;
        this.transform.parent = viewTransform;
    }

    public virtual void OnDrop() {
        GetComponent<BoxCollider>().enabled = true;
        this.owner = null;
        this.transform.parent = null;
    }
}