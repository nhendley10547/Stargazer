using UnityEngine;
using System;

public class Entity : MonoBehaviour {
    [NonSerialized]
    public Vector3 velocity;

    [NonSerialized]
    public Vector3 direction;

    [NonSerialized]
    public Vector3 position;

    [NonSerialized]
    public Equipment equipment;
	
	public LayerMask groundLayer; 

    public virtual void Death() {
        Destroy(this.gameObject);
    }
}