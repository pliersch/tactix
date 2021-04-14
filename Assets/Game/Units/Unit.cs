using Game.Battlefield;
using Game.Battlefield.Map;
using Game.Battlefield.Tanks;
using UnityEngine;

namespace Game.Units {

	public abstract class Unit {

		public Position Position { get; set; }
		public Vector3 RealPosition { get; set; }
		protected GameObject _go;
		protected TankActionHandler _actionHandler;
		private readonly Army _army;
		public int ActionPoints { get; set; }
		public int Health { get; set; }
		protected int _remainingActionPoints;
		internal int Damage;

		public Army Army {
			get { return _army; }
		}

		protected Unit(GameObject go, Army army, Position position, Vector3 realPosition) {
			Position = position;
			RealPosition = realPosition;
			_go = go;
			_army = army;
			_actionHandler = go.GetComponent<TankActionHandler>();
			_actionHandler.SetInteractionHandler(this);
		}

		public GameObject GetGameObject() {
			return _go;
		}

		public int GetRemainingActionPoints() {
			return _remainingActionPoints;
		}

		public void HandleClick() {
			_army.HandleUnitSelected(this);
		}

		public void HandleMovementComplete() {
			_army.HandleUnitMovementComplete(this);
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
			if (_remainingActionPoints == 0) {
				return;
			}
			_remainingActionPoints--;
			_go.transform.LookAt(target);
			TankShooting tankShooting = _go.GetComponent<TankShooting>();
			LineShooting shooting = _go.GetComponentInChildren<LineShooting>();
			Vector3 offset = target - _go.transform.position;
			float distance = Mathf.Sqrt(offset.x * offset.x + offset.z * offset.z);
			shooting.Shoot(distance);
			tankShooting.Fire(distance);
		}

		internal void DecreaseHealth(int damage) {
			Health -= damage;
			if (Health <= 0) {
				_army.HandleDeath(this);
				_go.GetComponent<TankHealth>().OnDeath();
			}
			_go.GetComponent<TankHealth>().TakeDamage(damage);
		}

		public void Highlight() {
			//			MeshRenderer[] renderers = _go.GetComponentsInChildren<MeshRenderer>();
			//			foreach (MeshRenderer renderer in renderers) {
			//				//t.material.color = Color.blue;
			//				Color color = renderer.material.color;
			//				color.a = 0.8f;
			//				renderer.material.color = color;
			//			}
		}

		public void UnHighlight() {
			//			MeshRenderer[] renderers = _go.GetComponentsInChildren<MeshRenderer>();
			//			foreach (MeshRenderer renderer in renderers) {
			//				//t.material.color = Color.blue;
			//				Color color = renderer.material.color;
			//				color.b = 1;
			//				renderer.material.color = color;
			//			}
		}

		public override string ToString() {
			return "Unit Pos: " + Position.x + "|" + Position.z;
		}
	}

}