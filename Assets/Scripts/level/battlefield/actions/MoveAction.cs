using UnityEngine;

namespace level.battlefield.actions {

	public class MoveAction : MonoBehaviour {

		public Transform _transform;
		public float duration;
		private float progress;

		private void Start() {
		}

		private void Update() {
			progress += Time.deltaTime / duration;
			if (progress > 1f) {
				progress = 1f;
			}


//			Vector3 position = spline.GetPoint(progress);
//			transform.localPosition = position;
//			transform.LookAt(position + spline.GetDirection(progress));
		}

		public void SetWay(Vector3[] position) {
			
		}

	}

}