using Game.Battlefield.Map;
using Game.Battlefield.Tanks;
using Game.Battlefield.util;
using UnityEngine;

namespace Game.Units {

	// TODO is it better to inherit from MonoBehaviour to use lifecycle methods? 
	public abstract class Unit {

		private readonly GameObject _go;
		private GameObject _infoText;
		protected int _remainingActionPoints;

		protected Unit(GameObject go, Army army, Position position, Vector3 realPosition) {
			Position = position;
			RealPosition = realPosition;
			_go = go;
			Army = army;
			// TODO "go" must have the TankActionHandler. possible nullpointer 
			TankActionHandler actionHandler = go.GetComponent<TankActionHandler>();
			actionHandler.SetInteractionHandler(this);

			_infoText = _go.GetComponentInChildren<Gui3DToCam>()._text;
			UnHighlight();
		}

		public Army Army { get; }

		public Position Position { get; private set; }

		public Vector3 RealPosition { get; private set; }

		protected int ActionPoints { get; set; }

		protected int Health { get; set; }

		public int Reach { get; protected set; }

		internal int Damage { get; set; }

		public GameObject GetGameObject() {
			return _go;
		}

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
			TankMovement movement = _go.GetComponent<TankMovement>();
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
			_go.transform.LookAt(target);
			TankShooting tankShooting = _go.GetComponent<TankShooting>();
			LineShooting shooting = _go.GetComponentInChildren<LineShooting>();
			Vector3 offset = target - _go.transform.position;
			var distance = Mathf.Sqrt(offset.x * offset.x + offset.z * offset.z);
			shooting.Shoot(distance);
			tankShooting.Fire(distance);
		}

		internal void DecreaseHealth(int damage) {
			Health -= damage;
			if (Health <= 0) {
				Army.HandleDeath(this);
				_go.GetComponent<TankHealth>().OnDeath();
			}

			_go.GetComponent<TankHealth>().TakeDamage(damage);
		}

		public void Highlight() {
			_infoText.SetActive(true);
		}

		public void UnHighlight() {
			_infoText.SetActive(false);
		}

		public override string ToString() {
			return "Unit Pos: " + Position.x + "|" + Position.z;
		}

	}

}