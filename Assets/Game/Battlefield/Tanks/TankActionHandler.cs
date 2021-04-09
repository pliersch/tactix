using Game.Battlefield.Cameras;
using Game.Units;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Battlefield.Tanks {

	public class TankActionHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

		private CameraFollow _cameraFollow;
		private Unit _handler;
		private Vector3 _cameraPosition;

		private void Start() {
			var camerRig = GameObject.FindWithTag("MainCamera");
			_cameraFollow = camerRig.GetComponent<CameraFollow>();
		}

		public void SetInteractionHandler(Unit handler) {
			_handler = handler;
		}

		private void OnMouseOver() {
			//Debug.Log("mouse over " + gameObject.name);
		}

		public void OnPointerDown(PointerEventData eventData) {
			_cameraPosition = _cameraFollow.transform.position;
		}

		public void OnPointerUp(PointerEventData eventData) {
			if (_cameraPosition != _cameraFollow.transform.position) {
				return;
			}
			_cameraFollow.AddTarget(gameObject.transform);
			_handler.HandleClick();
		}

		public void OnMovementComplete() {
			_handler.HandleMovementComplete();
		}

	}

}