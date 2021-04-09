using UnityEngine;

namespace level.battlefield {

	public class Tile : MonoBehaviour {

		public Color _colorOver;
		public Color _colorOut;
		private MeshRenderer _meshRenderer;
		private bool _isOver;
		private Position _position;
		private ITileActionHandler _handler;

		private void Start() {
			_meshRenderer = gameObject.GetComponent<MeshRenderer>();
			_meshRenderer.material.color = _colorOut;
			_isOver = false;
		}

		public void SetText(string msg) {
			//GetComponentInChildren<TextMesh>().text = msg;
		}

		private void OnMouseOver() {
			if (_isOver) {
				return;
			}
			_isOver = true;
			_meshRenderer.material.color = _colorOver;
		}

		private void OnMouseDown() {
		}

		private void OnMouseUp() {
			_handler.HandleTargetFieldSelected(_position);
		}

		private void OnMouseExit() {
			_isOver = false;
			_meshRenderer.material.color = _colorOut;
		}

		public void SetActionHandler(ITileActionHandler handler) {
			_handler = handler;
		}

		public void SetPosition(Position fieldPosition) {
			_position = fieldPosition;
		}

	}

}