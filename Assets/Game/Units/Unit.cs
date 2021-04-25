using Game.Battlefield.Map;
using Game.Battlefield.Tanks;
using UnityEngine;

namespace Game.Units {

	// TODO is it better to inherit from MonoBehaviour to use lifecycle methods? 
	public abstract class Unit {

		// private readonly GameObject _go;
		// private readonly GameObject _infoText;
		protected int _remainingActionPoints;

		protected Unit(Army army) {
			Army = army;
		}

		protected Unit(GameObject go, Army army, Position position, Vector3 realPosition) {
			Position = position;
			RealPosition = realPosition;
			// _go = go;
			Army = army;
			// TODO "go" must have the TankActionHandler. possible nullpointer 
			TankActionHandler actionHandler = go.GetComponent<TankActionHandler>();
			actionHandler.SetInteractionHandler(this);

			// _infoText = _go.GetComponentInChildren<Gui3DToCam>()._text;
			UnHighlight();
		}

		public Army Army { get; }

		public Position Position { get; set; }

		public Vector3 RealPosition { get; set; }

		public GameObject GameObject { get; set; }

		protected int ActionPoints { get; set; }

		protected int Health { get; set; }

		public int Reach { get; protected set; }

		internal int Damage { get; set; }

		public int GetRemainingActionPoints() {
			return _remainingActionPoints;
		}

		public void HandleClick() {
			Army.HandleUnitSelected(this);
		}

		public void HandleMovementComplete() {
			Army.HandleUnitMovementComplete(this);
		}

		public void Move(Field[] way) {
			TankMovement movement = GameObject.GetComponent<TankMovement>();
			movement.enabled = true;
			movement.Move(way);
			Field targetField = way[way.Length - 1];
			Position = targetField.Position;
			RealPosition = targetField.RealPosition;
			_remainingActionPoints = targetField.RemainedActionPoint;
		}

		public void ResetActionPoints() {
			_remainingActionPoints = ActionPoints;
		}

		public void Fire(Vector3 target) {
			if (_remainingActionPoints == 0) return;
			_remainingActionPoints--;
			GameObject.transform.LookAt(target);
			TankShooting tankShooting = GameObject.GetComponent<TankShooting>();
			LineShooting shooting = GameObject.GetComponentInChildren<LineShooting>();
			Vector3 offset = target - GameObject.transform.position;
			var distance = Mathf.Sqrt(offset.x * offset.x + offset.z * offset.z);
			shooting.Shoot(distance);
			tankShooting.Fire(distance);
		}

		internal void DecreaseHealth(int damage) {
			Health -= damage;
			if (Health <= 0) {
				Army.HandleDeath(this);
				GameObject.GetComponent<TankHealth>().OnDeath();
			}

			GameObject.GetComponent<TankHealth>().TakeDamage(damage);
		}

		public void Highlight() {
			// _infoText.SetActive(true);
		}

		public void UnHighlight() {
			// _infoText.SetActive(false);
		}

		public override string ToString() {
			return "Unit Pos: " + Position.x + "|" + Position.z;
		}

	}

}