using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlowField
{
	public class UnitController : MonoBehaviour
	{
		public GridController gridController;
		public GameObject unitPrefab;
		public int numUnitsPerSpawn;
		public float moveSpeed;

		private List<GameObject> unitsInGame;

		private void Awake()
		{
			unitsInGame = new List<GameObject>();
		}

		void Update()
		{

		}

		private void FixedUpdate()
		{
			if (gridController._currentFlowField == null) { return; }
			foreach (GameObject unit in unitsInGame)
			{
				Cell cellBelow = gridController._currentFlowField.GetCellFromWorldPos(unit.transform.position);
				Vector3 moveDirection = new Vector3(cellBelow._bestDirection.Vector.x, 0, cellBelow._bestDirection.Vector.y);
				Rigidbody unitRB = unit.GetComponent<Rigidbody>();
				unitRB.velocity = moveDirection * moveSpeed;
			}
		}

		public void SpawnUnits()
		{
			Vector2Int gridSize = gridController._gridSize;
			float nodeRadius = gridController._cellRadius;
			Vector2 maxSpawnPos = new Vector2(gridSize.x * nodeRadius * 2 + nodeRadius, gridSize.y * nodeRadius * 2 + nodeRadius);
			int colMask = LayerMask.GetMask("Impassible", "Units");
			Vector3 newPos;
			for (int i = 0; i < numUnitsPerSpawn; i++)
			{
				GameObject newUnit = Instantiate(unitPrefab);
				newUnit.transform.parent = transform;
				unitsInGame.Add(newUnit);
				do
				{
					newPos = new Vector3(Random.Range(0, maxSpawnPos.x), 0.6f, Random.Range(0, maxSpawnPos.y));
					newUnit.transform.position = newPos;
				}
				while (Physics.OverlapSphere(newPos, 0.25f, colMask).Length > 0);
			}
		}

		public void DestroyUnits()
		{
			foreach (GameObject go in unitsInGame)
			{
				Destroy(go);
			}
			unitsInGame.Clear();
		}
	}

}

