using System.Collections.Generic;
using Game.Battlefield.Map;

namespace Game.Units {

	public class Army {

		private readonly List<Unit> _units;
		private Unit _activeUnit;

		public Army(List<Unit> units) {
			_units = units;
		}
		
		public Map Map { get; set; }

		public Unit GetActiveUnit() {
			return _activeUnit;
		}

		public List<Unit> GetUnits() {
			return _units;
		}

		public void ComputeNextActiveUnit() {
			int index = _units.IndexOf(_activeUnit);
			int newIndex = getNextUnitInList(index);
			HandleUnitSelected(_units[newIndex]);
		}

		private int getNextUnitInList(int index) {
			if (index == _units.Count - 1) {
				return 0;
			}
			return index + 1;
		}

		public void HandleUnitSelected(Unit unit) {
			_activeUnit = unit;
			Map.HandleUnitSelected(unit);
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
			Map.HandleUnitMovementComplete(unit);
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
			Map.HandleDeath(unit);
		}


	}

}