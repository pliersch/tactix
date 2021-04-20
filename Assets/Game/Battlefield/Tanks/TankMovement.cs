using System;
using Game.Battlefield.Map;
using UnityEngine;

namespace Game.Battlefield.Tanks {

	public class TankMovement : MonoBehaviour {

		// Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
		public AudioSource _movementAudio;

		public AudioClip _engineDriving; // Audio to play when the tank is moving.
		private Rigidbody _rigidbody; // Reference used to move the tank.
		private int _currentFieldIndex;
		private Vector3 _checkPoint;
		private Field[] _way;
		private const float Speed = 0.5f;
		private Vector3 _movement;
		private bool _isMoving;

		private void Awake() {
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void OnEnable() {
			_isMoving = false;
			_rigidbody.isKinematic = false;
			_movement = Vector3.zero;
		}

		private void OnDisable() {
			_rigidbody.isKinematic = true;
		}

		private void Update() {
			if (!_isMoving) {
				return;
			}
			_rigidbody.transform.position += _movement;
			if (IsCheckPointReached()) {
				if (IsTargetFieldReached()) {
					StopMoving();
				} else {
					SetNextCheckPoint();
					Turn2NextCheckPoint();
				}
			}
		}

		public void Move(Field[] way) {
			_isMoving = true;
			_way = way;
			_currentFieldIndex = 0;
			_movementAudio.Play();
			SetNextCheckPoint();
			Turn2NextCheckPoint();
		}

		private bool IsTargetFieldReached() {
			return _currentFieldIndex == _way.Length - 1;
		}

		private void StopMoving() {
			_isMoving = false;
			_movement = Vector3.zero;
			_movementAudio.Stop();
			enabled = false;
			GetComponentInParent<TankActionHandler>().OnMovementComplete();
		}

		private void SetNextCheckPoint() {
			_checkPoint = _way[++_currentFieldIndex].RealPosition;
		}

		private void Turn2NextCheckPoint() {
			Position nextPosition = _way[_currentFieldIndex].Position;
			Position lastPosition = _way[_currentFieldIndex - 1].Position;
			if (nextPosition.x > lastPosition.x) {
				_rigidbody.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
				_movement = new Vector3(Speed, 0, 0);
			} else if (nextPosition.x < lastPosition.x) {
				_rigidbody.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
				_movement = new Vector3(-Speed, 0, 0);
			} else if (nextPosition.z > lastPosition.z) {
				_rigidbody.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				_movement = new Vector3(0, 0, Speed);
			} else if (nextPosition.z < lastPosition.z) {
				_rigidbody.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
				_movement = new Vector3(0, 0, -Speed);
			}
		}

		private bool IsCheckPointReached() {
			return Math.Abs(_rigidbody.transform.position.x - _checkPoint.x) < 0.01f &&
			       Math.Abs(_rigidbody.transform.position.z - _checkPoint.z) < 0.01f;
		}

	}

}