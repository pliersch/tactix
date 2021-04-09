using UnityEngine;

namespace Game.Battlefield.Tanks {

	public class LineShooting : MonoBehaviour {

		public int _damagePerShot = 20; // The damage inflicted by each bullet.
		private float _timer; // A timer to determine when to fire.
		private ParticleSystem _gunParticles; // Reference to the particle system.
		private LineRenderer _gunLine; // Reference to the line renderer.
		private AudioSource _gunAudio; // Reference to the audio source.
		private Light _gunLight; // Reference to the light component.

		//public Light faceLight; // do we need this light?
		float effectsDisplayTime = 0.1f; // The proportion of the _timeBetweenBullets that the effects will display for.

		private void Awake() {
			// Set up the references.
			_gunParticles = GetComponent<ParticleSystem>();
			_gunLine = GetComponent<LineRenderer>();
			_gunAudio = GetComponent<AudioSource>();
			_gunLight = GetComponent<Light>();
			//faceLight = GetComponentInChildren<Light> ();
		}

		private void Update() {
			// Add the time since Update was last called to the timer.
			_timer += Time.deltaTime;
			// If the timer has exceeded the proportion of _timeBetweenBullets that the effects should be displayed for...
			if (_timer >= effectsDisplayTime) {
				// ... disable the effects.
				DisableEffects();
			}
		}

		public void DisableEffects() {
			// Disable the line renderer and the light.
			_gunLine.enabled = false;
			//faceLight.enabled = false; // do we need this light?
			_gunLight.enabled = false;
		}

		public void Shoot(float distance) {
			_timer = 0f;
			_gunAudio.Play();
			_gunLight.enabled = true;
//			faceLight.enabled = true; // do we need this light?
			_gunParticles.Stop();
			_gunParticles.Play();
			_gunLine.enabled = true;
			//_gunLine.SetPosition(0, transform.localPosition);

			_gunLine.SetPosition(1, new Vector3(0, 0, distance));
		}

	}

}