using UnityEngine;

namespace Game.Battlefield.util {

	public class Gui3DToCam : MonoBehaviour {

		public GameObject _text;

		private void Update() {
			Vector3 dir = Quaternion.Euler(0, 270, 45) * Vector3.down;
			Quaternion rotation = Quaternion.LookRotation(dir);
			_text.transform.rotation = rotation;
		}

	}

}