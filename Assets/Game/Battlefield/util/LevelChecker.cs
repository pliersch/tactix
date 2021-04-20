using System;
using Game.Battlefield.Map;
using UnityEngine;

namespace Game.Battlefield.util {

	public class LevelChecker : MonoBehaviour {

		public float CheckTileSize(GameObject tile) {
			Renderer fieldRenderer = tile.GetComponentInChildren<Renderer>();
			Vector3 boundsSize = fieldRenderer.bounds.size;
			float width = boundsSize.x;
			float depth = boundsSize.z;
			if (Math.Abs(width - depth) > 0.01) {
				Debug.LogError("Computed size of tile: " + width + " and " + depth);
			}
			return width;
		}

		public void FindOccupiedFields(Field[,] fields, PrefabFactory factory) {
			foreach (Field field in fields) {
				GameObject go =
					Instantiate(factory.tile, field.RealPosition, factory.tile.transform.rotation);
				Tile tile = (Tile)go.GetComponent(typeof(Tile));
				ExplosionDamage(field, tile);
			}
		}

		void ExplosionDamage(Field field, Tile tile) {
			Collider[] hitColliders = Physics.OverlapSphere(tile.transform.position, 1);
			int i = 0;
			while (i < hitColliders.Length) {
				if (hitColliders[i].CompareTag("Wall")) {
					field.IsFree = false;
				}
				i++;
			}
			Destroy(tile.gameObject);
		}

	}

}