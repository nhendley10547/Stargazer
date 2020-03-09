using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeGrid : MonoBehaviour {

	public bool displayGizmos;

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake() {
		this.nodeDiameter = this.nodeRadius * 2;
		this.gridSizeX = Mathf.RoundToInt(this.gridWorldSize.x/this.nodeDiameter);
		this.gridSizeY = Mathf.RoundToInt(this.gridWorldSize.y/this.nodeDiameter);
		this.CreateGrid();
	}

	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	void CreateGrid() {
		this.grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * this.gridWorldSize.x/2 - Vector3.forward * this.gridWorldSize.y/2;
		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				Vector3 worldPoint = worldBottomLeft + 
					Vector3.right * (x * this.nodeDiameter + this.nodeRadius) + 
					Vector3.forward * (y * this.nodeDiameter + this.nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

				this.grid[x, y] = new Node(walkable, worldPoint, x, y, 0);
			}
		}

		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				Node node = this.grid[x, y];
				if (!node.walkable) continue;
				List<Node> neighbors = GetNeighbors(node);
				int numberOfWalkable = neighbors.Count;
				node.weight += (8 - GetNeighbors(node).Count) * 2;
			}
		}
	}

	void BlurWeightMap(int blurSize) {
		int kernelSize = blurSize * 2 + 1;
		int kernelExtents = (kernelSize - 1) / 2;

		int[,] weightHozPass = new int[gridSizeX, gridSizeY];
		int[,] weightVerPass = new int[gridSizeX, gridSizeY];
		for (int y = 0; y < gridSizeY; y++) {
			for (int x = -kernelExtents; x <= kernelExtents; x++) {
				int sampleX = Mathf.Clamp(x, 0, kernelExtents);

			}
		}
	}

	public List<Node> GetNeighbors(Node node) {
		List<Node> neighbors = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0) continue;
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					if (!this.grid[checkX, checkY].walkable) continue;
					neighbors.Add(this.grid[checkX, checkY]);
				}
			}
		}

		return neighbors;
	}

	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + this.gridWorldSize.x/2) / this.gridWorldSize.x;
		float percentY = (worldPosition.z + this.gridWorldSize.y/2) / this.gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		
		int x = Mathf.RoundToInt((this.gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((this.gridSizeY - 1) * percentY);
		return this.grid[x, y];
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
		
		if (this.grid != null && displayGizmos) {
			foreach(Node n in grid) {
				Gizmos.color = n.walkable ? Color.white : Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (this.nodeDiameter - .1f));
				// if (n.walkable)
				// 	Handles.Label(n.worldPosition, n.weight.ToString());
			}
		}
		
	}
}
