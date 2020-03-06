using UnityEngine;

public abstract class Equipment : MonoBehaviour {
    
    public Entity ownerEntity;

    public abstract void OnActivate();

}