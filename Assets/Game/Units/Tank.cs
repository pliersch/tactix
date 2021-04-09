using level.battlefield;
using UnityEngine;

namespace level.gameObjects {

	public class Tank : Unit {

		public Tank(GameObject go, Army army, Position position, Vector3 realPosition) : base(go, army, position, realPosition) {
			ActionPoints = 5;
			Health = 10;
			Damage = 2;
			_remainingActionPoints = ActionPoints;
		}

	}

}