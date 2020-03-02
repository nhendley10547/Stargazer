using UnityEngine;

public class Entity : MonoBehaviour {
    public Vector3 velocity;
    public Vector3 direction;
    public Vector3 position;

	protected LayerMask groundLayer; 
    protected LayerMask itemLayer;

    protected virtual void Start() {
        groundLayer = LayerMask.NameToLayer("Ground");
        itemLayer = LayerMask.NameToLayer("Item");
        print("Hello");
    }
}