using System.Collections.Generic;
using Game.Units;
using UnityEngine;

namespace Game.Battlefield.util {

	public static class Raycaster {

		public static List<Unit> FindPossibleTargets(Unit attacker, IEnumerable<Unit> enemies) {
			List<Unit> possibleTargets = new List<Unit>();
			Transform p1 = attacker.GetGameObject().transform;
			Vector3 attackerPosition = attacker.GetGameObject().transform.position;
			GameObject attackerCopy = new GameObject();
			attackerCopy.transform.position = attackerPosition;
			
			foreach (Unit enemy in enemies) {
				attackerCopy.transform.LookAt(enemy.RealPosition);
				bool isWall = false;
				var dist = Vector3.Distance(attackerPosition, enemy.GetGameObject().transform.position);
				
				
				
				RaycastHit[] hits = Physics.RaycastAll(attackerCopy.transform.position, attackerCopy.transform.forward, dist);
				foreach (RaycastHit hit in hits)
					if (hit.collider.CompareTag("Wall"))
						isWall = true;
				if (!isWall) possibleTargets.Add(enemy);
			}

			return possibleTargets;
		}

	}

}