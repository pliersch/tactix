using UnityEngine;

namespace Game.Battlefield.Tanks {

	public class TankShooting : MonoBehaviour {

		public Rigidbody _shell; // Prefab of the shell.
		public Transform _fireTransform; // A child of the tank where the shells are spawned.
		public AudioSource _shootingAudio; // Reference to the audio source used to play the shooting audio.
		public AudioClip _fireClip; // Audio that plays when each shot is fired.

		private void Start() {
			_shootingAudio.clip = _fireClip;
		}

		private void Update() {
		}

		public void Fire(float distance) {
			// Create an instance of the shell and store a reference to it's rigidbody.
			Rigidbody shellInstance =
				Instantiate(_shell, _fireTransform.position, _fireTransform.rotation);

			// Set the shell's velocity to the launch force in the fire position's forward direction.
			shellInstance.velocity = distance * _fireTransform.forward;
			_shootingAudio.Play();
		}

	}

}