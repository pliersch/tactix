using System.Collections.Generic;
using Game.Battlefield.util;
using Game.Units;
using UnityEngine;

namespace Game.Battlefield.Map {

	public class MapModel : MonoBehaviour {

		private Pathfinder _pathfinder;

		private Field[,] _fields;

		private float _tileSize;

		public void SetFields(Field[,] fields) {
			_fields = fields;
			_pathfinder = new Pathfinder(fields);
		}
		
		public void SetTileSize(float tileSize) {
			_tileSize = tileSize;
		}
		// TODO do we need param unit?
		public void UpdateAddedUnit(Unit unit, Position position) {
			_fields[position.x, position.z].IsFree = false;
		}

		public Field GetField(Position position) {
			return _fields[position.x, position.z];
		}

		internal Field[] GetReachableFields(Position position, int actionPoints) {
			Field[] fields = _pathfinder.GetReachableFields(position, actionPoints);
			return fields;
		}

		public Position ConvertCoordinateToPosition(Vector3 coordinate) {
			int x = (int)((coordinate.x - _tileSize / 2) / _tileSize);
			int z = (int)((coordinate.z - _tileSize / 2) / _tileSize);
			return new Position(x, z);
		}


		//		public Vector3 ConvertPositionToCoordinate(int x, int z) {
		//			float center = _tileSize / 2;
		//			float xPos = x * _tileSize + center;
		//			float zPos = z * _tileSize + center;
		//			return new Vector3(xPos, 0, zPos);
		//
		//		}

		//		private Position ConvertPositionToCoordinate(Position position) {
		//
		//		}
		public Field[] GetWay(Position targetPosition) {
			List<Field> endToStart = new List<Field>();
			Field current = GetField(targetPosition);
			endToStart.Add(current);
			Field parent = current.Parent;
			while (parent != null) {
				endToStart.Add(parent);
				parent = parent.Parent;
			}
			endToStart.Reverse();
			return endToStart.ToArray();
		}

		public void UpdateFreeField(Field field, bool isFree) {
			field.IsFree = isFree;
		}

		public void UpdateFreeFields(Field[] way) {
			way[0].IsFree = true;
			way[way.Length - 1].IsFree = false;
		}

		public float GetDistance(Position pos1, Position pos2) {
			float x = Mathf.Abs(pos1.x - pos2.x) * _tileSize;
			float z = Mathf.Abs(pos1.z - pos2.z) * _tileSize;
			return Mathf.Sqrt(x * x + z * z);
		}

		public Vector3 GetDistance(Vector3 pos1, Vector3 pos2) {
			return pos2 - pos1;
		}

	}

}