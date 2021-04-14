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
			Field chckField;

			var startField = _fields[position.x, position.z];
			var currentField = startField;
			currentField.RemainedActionPoint = actionPoints;
			_closedFields.Add(currentField);

			var reachableNeighbours = GetReachableNeighbours(currentField, actionPoints);
			if (reachableNeighbours.Count > 0) {
				for (int i = 0; i < reachableNeighbours.Count; i++) {
					chckField = reachableNeighbours[i];
					_openFields.Add(chckField);
					chckField.Parent = currentField;
					chckField.RemainedActionPoint = actionPoints - chckField.WayCost;
				}
			}
			while (_openFields.Count > 0) {
				currentField = _openFields[_openFields.Count - 1];
				_openFields.RemoveAt(_openFields.Count - 1);

				reachableNeighbours = GetReachableNeighbours(currentField, currentField.RemainedActionPoint);
				for (int j = 0; j < reachableNeighbours.Count; j++) {
					chckField = reachableNeighbours[j];
					if (IsNewField(chckField)) {
						_openFields.Add(chckField);
						chckField.Parent = currentField;
						chckField.RemainedActionPoint = currentField.RemainedActionPoint - chckField.WayCost;
					} else {
						if (currentField.RemainedActionPoint - chckField.WayCost > chckField.RemainedActionPoint) {
							chckField.RemainedActionPoint = currentField.RemainedActionPoint - chckField.WayCost;
							chckField.Parent = currentField;
							if (IsInClosedList(chckField)) {
								int index = _closedFields.IndexOf(chckField);
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
					Field chckField = _fields[neighboursPoints[i].x, neighboursPoints[i].z];
					if (chckField.IsFree && remainingActionPoints >= chckField.WayCost) {
						neighboursFields.Add(chckField);
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