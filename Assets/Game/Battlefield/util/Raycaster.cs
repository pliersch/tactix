using System.Collections.Generic;
using Game.Units;
using UnityEngine;

namespace Game.Battlefield.util {

	public static class Raycaster {

		public static List<Unit> FindPossibleTargets(Unit attacker, IEnumerable<Unit> enemies) {
			List<Unit> list = new List<Unit>();
			foreach (Unit enemy in enemies) {
				var isWall = false;
				Transform p1 = attacker.GetGameObject().transform;
				GameObject go = new GameObject("test");
				go.transform.position = p1.position;
				go.transform.LookAt(enemy.RealPosition);
				var dist = Vector3.Distance(p1.position, enemy.GetGameObject().transform.position);
				RaycastHit[] hits = Physics.RaycastAll(go.transform.position, go.transform.forward, dist);
				foreach (RaycastHit hit in hits)
					if (hit.collider.CompareTag("Wall"))
						isWall = true;
				if (!isWall) list.Add(enemy);
			}

			return list;
		}

	}

}