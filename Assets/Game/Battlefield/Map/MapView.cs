using System.Collections.Generic;
using UnityEngine;

namespace Game.Battlefield.Map {

	public class MapView : MonoBehaviour, ITileActionHandler {

		public GameObject _fieldPrefab;
		private IBattlefieldViewController _controller;
		private List<GameObject> _fields;
		
		public GameObject AddUnit(GameObject unit, Vector3 position) {
			return Instantiate(unit, position, unit.transform.rotation);
			//			GameObject go = Instantiate(unit, position, unit.transform.rotation);
		}

		// ReSharper disable Unity.PerformanceAnalysis
		internal void ShowReachableFields(IEnumerable<Field> reachableFields) {
			_fields = new List<GameObject>();
			foreach (Field field in reachableFields) {
				GameObject go =
					Instantiate(_fieldPrefab, field.RealPosition, _fieldPrefab.transform.rotation);
				Tile tile = (Tile) go.GetComponent(typeof(Tile));
				// TODO set controller as actionhandler maybe
				tile.SetActionHandler(this);
				tile.SetPosition(field.Position);
				tile.SetText(field.Position.x + " | " + field.Position.z);
				_fields.Add(go);
			}
		}

		public void HandleTargetFieldSelected(Position position) {
			_controller.HandleTargetFieldSelected(position);
		}

		public void SetController(IBattlefieldViewController controller) {
			_controller = controller;
		}

		public void DestroyReachableFields() {
			if (_fields == null) 
				return;
			foreach (GameObject field in _fields) {
				Destroy(field);
			}
		}
	}
}