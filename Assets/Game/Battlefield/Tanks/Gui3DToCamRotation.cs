using UnityEngine;

namespace Game.Battlefield.Tanks {

	public class Gui : MonoBehaviour {

		public GameObject _text;
		
		private void Update() {
			ComputeDirection();
		}

		private void ComputeDirection() {
			Vector3 dir = Quaternion.Euler(0, 270, 45) * Vector3.down;
			Quaternion rotation = Quaternion.LookRotation(dir);
			_text.transform.rotation = rotation;
		}

	}

}