using System;
using UnityEngine;

namespace level.battlefield.util {

	public class LevelChecker : MonoBehaviour {

		public float CheckTileSize(GameObject tile) {
			Renderer fieldRenderer = tile.GetComponentInChildren<Renderer>();
			float width = fieldRenderer.bounds.size.x;
			float depth = fieldRenderer.bounds.size.z;
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