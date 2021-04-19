using System.Collections.Generic;
using Game.Units;
using UnityEngine;

namespace Game.Battlefield.util {

	public static class Raycaster {

		public static List<Unit> FindPossibleTargets(Unit attacker, IEnumerable<Unit> enemies) {
			List<Unit> possibleTargets = new List<Unit>();
			GameObject attackerCopy = createAttackerCopy(attacker);
			foreach (Unit enemy in enemies) {
				attackerCopy.transform.LookAt(enemy.RealPosition);
				float distance = ComputeDistance(attackerCopy, enemy.GetGameObject());

				if (distance > attacker.Reach) {
					continue;
				}
				// Debug.DrawLine(attackerCopy.transform.position, enemy.GetGameObject().transform.position, Color.red, 7);

				if (isEnemyVisible(attackerCopy, distance)) {
					possibleTargets.Add(enemy);
				}
			}

			return possibleTargets;
		}

		private static float ComputeDistance(GameObject attacker, GameObject enemy) {
			return Vector3.Distance(attacker.transform.position, enemy.transform.position);
		}

		private static GameObject createAttackerCopy(Unit attacker) {
			Vector3 attackerPosition = attacker.GetGameObject().transform.position;
			GameObject attackerCopy = new GameObject();
			attackerCopy.transform.position = attackerPosition;
			return attackerCopy;
		}

		private static RaycastHit[] ComputeRaycastHits(GameObject go, float distance) {
			return Physics.RaycastAll(go.transform.position, go.transform.forward, distance);
		}

		private static bool isEnemyVisible(GameObject attacker, float distance) {
			RaycastHit[] hits = ComputeRaycastHits(attacker, distance);
			bool isWall = false;
			foreach (RaycastHit hit in hits) {
				if (hit.collider.CompareTag("Wall"))
					isWall = true;
				break;
			}

			return !isWall;
		}

	}

}