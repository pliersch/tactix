using Game.Battlefield;
using System;
using System.Collections.Generic;
using Game.Battlefield.Map;

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
			//index++;
			return index + 1;
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