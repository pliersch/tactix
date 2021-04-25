namespace Game.Units {

	public class Tank : Unit {
		
		// public Tank(GameObject go, Army army, Position position, Vector3 realPosition) : base(go, army, position, realPosition) {
		// 	ActionPoints = 5;
		// 	Health = 10;
		// 	Damage = 2;
		// 	Reach = 35;
		// 	_remainingActionPoints = ActionPoints;
		// }		
		
		public Tank(Army army) : base(army) {
			ActionPoints = 5;
			Health = 10;
			Damage = 2;
			Reach = 35;
			_remainingActionPoints = ActionPoints;
		}

	}

}