using System.Collections;
using System.Collections.Generic;
using level.gameObjects;
using UnityEngine;

namespace level.battlefield.util {

	public class Raycaster {

		public List<Unit> FindPossibleTargets(Unit offener, List<Unit> enemies) {
		List<Unit> list = new List<Unit>();
			foreach (Unit enemy in enemies) {
				bool isWall = false;
				Transform p1 = offener.GetGameObject().transform;
				var go = new GameObject("test");
				go.transform.position = p1.position;
				go.transform.LookAt(enemy.RealPosition);
				float dist = Vector3.Distance(p1.position, enemy.GetGameObject().transform.position);
				RaycastHit[] hits = Physics.RaycastAll(go.transform.position, go.transform.forward, dist);
				foreach (RaycastHit hit in hits) {
					if (hit.collider.CompareTag("Wall")) {
						isWall = true;
					}
				}
				if (!isWall) {
					list.Add(enemy);
				}
			}
			return list;
		}

	}

}