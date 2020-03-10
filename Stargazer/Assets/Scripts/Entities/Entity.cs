using UnityEngine;

public class Entity : MonoBehaviour {
    public Vector3 velocity;
    public Vector3 direction;
    public Vector3 position;
    public Equipment equipment;
	
	public LayerMask groundLayer; 

    public virtual void Death() {
        Destroy(this.gameObject);
    }
}