using UnityEngine;

namespace Game.Battlefield.Tanks {

	public class Gui : MonoBehaviour {

		public GameObject _text;

		private void Start() {
			Debug.Log(Camera.main.transform.rotation);
		}

		private void Update() {
			// Debug.Log(Camera.main.transform.position);
			Vector3 cam = new Vector3(0, 45, 0);
			_text.transform.LookAt(cam);
		}

	}

}