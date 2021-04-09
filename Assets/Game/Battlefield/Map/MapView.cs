using UnityEngine;

namespace Game.Battlefield {

	public class MapView : MonoBehaviour, ITileActionHandler {

		public GameObject _fieldPrefab;
		private IBattlefieldViewController _controller;

		public GameObject AddUnit(GameObject unit, Vector3 position) {
			return Instantiate(unit, position, unit.transform.rotation);
			//			GameObject go = Instantiate(unit, position, unit.transform.rotation);
		}

		internal void ShowReachableFields(Field[] reachableFields) {
			foreach (var field in reachableFields) {
				GameObject go =
					Instantiate(_fieldPrefab, field.RealPosition, _fieldPrefab.transform.rotation);
				Tile tile = (Tile)go.GetComponent(typeof(Tile));
				// TODO set controller as actionhandler maybe
				tile.SetActionHandler(this);
				tile.SetPosition(field.Position);
				tile.SetText(field.Position.x + " | " + field.Position.z);
			}
		}

		public void HandleTargetFieldSelected(Position position) {
			_controller.HandleTargetFieldSelected(position);
		}

		public void SetController(IBattlefieldViewController controller) {
			_controller = controller;
		}

		public void DestroyReachableFields() {
			foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Field")) {
				Destroy(tile);
			}
		}

	}

}