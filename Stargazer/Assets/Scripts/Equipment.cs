using UnityEngine;

public abstract class Equipment : MonoBehaviour {
    
    protected Entity owner;

    public abstract void OnActivate();

    public virtual void OnEquip(Entity owner, Transform viewTransform) {
        
        this.owner = owner;
        this.transform.parent = viewTransform;
    }

    public virtual void OnDrop() {
        this.owner = null;
        this.transform.parent = null;
    }
}