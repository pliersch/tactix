using System.Collections.Generic;
using Game.Battlefield.Map;

namespace Game.Battlefield.util {

	public class Pathfinder {

		private readonly Field[,] _fields;
		private List<Field> _openFields;
		private List<Field> _closedFields;
		private readonly int _rows;
		private readonly int _columns;

		public Pathfinder(Field[,] fields) {
			_fields = fields;
			_rows = _fields.GetLength(0);
			_columns = _fields.GetLength(1);
		}

		public Field[] GetReachableFields(Position position, int actionPoints) {
			if (actionPoints == 0) {
				return new Field[0];
			}
			CleanUpParents();
			_openFields = new List<Field>();
			_closedFields = new List<Field>();
			Field checkField;

			Field startField = _fields[position.x, position.z];
			Field currentField = startField;
			currentField.RemainedActionPoint = actionPoints;
			_closedFields.Add(currentField);

			var reachableNeighbours = GetReachableNeighbours(currentField, actionPoints);
			if (reachableNeighbours.Count > 0) {
				for (int i = 0; i < reachableNeighbours.Count; i++) {
					checkField = reachableNeighbours[i];
					_openFields.Add(checkField);
					checkField.Parent = currentField;
					checkField.RemainedActionPoint = actionPoints - checkField.WayCost;
				}
			}
			while (_openFields.Count > 0) {
				currentField = _openFields[_openFields.Count - 1];
				_openFields.RemoveAt(_openFields.Count - 1);

				reachableNeighbours = GetReachableNeighbours(currentField, currentField.RemainedActionPoint);
				for (int j = 0; j < reachableNeighbours.Count; j++) {
					checkField = reachableNeighbours[j];
					if (IsNewField(checkField)) {
						_openFields.Add(checkField);
						checkField.Parent = currentField;
						checkField.RemainedActionPoint = currentField.RemainedActionPoint - checkField.WayCost;
					} else {
						if (currentField.RemainedActionPoint - checkField.WayCost > checkField.RemainedActionPoint) {
							checkField.RemainedActionPoint = currentField.RemainedActionPoint - checkField.WayCost;
							checkField.Parent = currentField;
							if (IsInClosedList(checkField)) {
								int index = _closedFields.IndexOf(checkField);
								_openFields.Add(_closedFields[index]);
								_closedFields.RemoveAt(index);
							}
						}
					}
				}
				_closedFields.Add(currentField);
			}
			_closedFields.Remove(startField);
			return _closedFields.ToArray();
		}

		private bool IsInOpenList(Field field) {
			return _openFields.Contains(field);
		}

		private bool IsInClosedList(Field field) {
			return _closedFields.Contains(field);
		}

		private bool IsNewField(Field field) {
			return !IsInOpenList(field) && !IsInClosedList(field);
		}

		private List<Field> GetReachableNeighbours(Field field, int remainingActionPoints) {
			Position[] neighboursPoints = GetNeighbourPositions(field.Position);
			List<Field> neighboursFields = new List<Field>();

			for (int i = 0; i < neighboursPoints.Length; i++) {
				if (ExitsField(neighboursPoints[i])) {
					Field checkField = _fields[neighboursPoints[i].x, neighboursPoints[i].z];
					if (checkField.IsFree && remainingActionPoints >= checkField.WayCost) {
						neighboursFields.Add(checkField);
					}
				}
			}
			return neighboursFields;
		}


		private bool ExitsField(Position position) {
			return position.x >= 0 && position.z >= 0 && position.x < _rows && position.z < _columns;
		}

		private Position[] GetNeighbourPositions(Position position) {
			Position[] neighbours = new Position[4];
			int xPos = position.x;
			int zPos = position.z;
			neighbours[0] = new Position(xPos, zPos + 1);
			neighbours[1] = new Position(xPos + 1, zPos);
			neighbours[2] = new Position(xPos, zPos - 1);
			neighbours[3] = new Position(xPos - 1, zPos);

			return neighbours;
		}

		// TODO better to safe reachable fields and clean up only these fields
		private void CleanUpParents() {
			foreach (Field field in _fields) {
				field.Parent = null;
			}
		}

	}

}