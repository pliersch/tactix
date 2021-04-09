using Game.Battlefield;
using System.Collections.Generic;

namespace Game.Units {

	public class Army {

		public List<Unit> _units;
		private Unit _activeUnit;
		private readonly Map _battlefield;

		public Army(Map battlefield, List<Unit> units) {
			_battlefield = battlefield;
			_units = units;
		}

		public Unit GetActiveUnit() {
			return _activeUnit;
		}

		public void HandleUnitSelected(Unit unit) {
			_activeUnit = unit;
			_battlefield.HandleUnitSelected(unit);
		}

		internal void MoveActiveUnit(Field[] way) {
			_activeUnit.Move(way);
		}

		public void ResetActionPoints() {
			foreach (Unit unit in _units) {
				unit.ResetActionPoints();
			}
		}

		public void HandleUnitMovementComplete(Unit unit) {
			_battlefield.HandleUnitMovementComplete(unit);
		}

		public List<Unit> GetUnits() {
			return _units;
		}

		public void HighlightUnits(List<Unit> units) {
			foreach (Unit unit in units) {
				unit.Highlight();
			}
		}

		public void UnHighlightUnits(Unit[] units) {
			foreach (Unit unit in units) {
				unit.UnHighlight();
			}
		}

		internal void HandleDeath(Unit unit) {
			_units.Remove(unit);
			_battlefield.HandleDeath(unit);
		}
	}

}