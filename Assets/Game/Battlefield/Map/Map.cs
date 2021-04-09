using Game.Battlefield.util;
using Game.Units;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Battlefield {

	public class Map : MonoBehaviour, IBattlefieldViewController {

		public MapModel _model;
		public MapView _view;
		public PrefabFactory _factory;
		public LevelChecker _levelChecker;
		public Button _nextTurnButton;
		public Button _exitButton;
		public Button _nextUnitButton;
		public int _rows;
		public int _columns;
		private Army _myArmy;
		private Army _enemyArmy;
		private Army _offenerArmy;
		private Army _defenderArmy;
		private List<Unit> _possibleTargets;

		// TODO: move LevelChecker and all of generation/initializing to a factory. Don´t want see here
		// TODO: better Use Unity Editor scripts and check it before compile (no code in final game)
		private void Start() {
			Debug.Log("foo");
			float tileSize = _levelChecker.CheckTileSize(_factory.tile);
			Field[,] fields = _model.GenerateFields(_rows, _columns, tileSize, transform.position);
			_nextTurnButton.onClick.AddListener(OnNextTurn);
			_exitButton.onClick.AddListener(OnExit);
			_nextUnitButton.onClick.AddListener(OnNextUnit);
			_levelChecker.FindOccupiedFields(fields, _factory);
			_view.SetController(this);
			AddUnits();
		}

		public void HandleUnitSelected(Unit unit) {
			// TODO re-enable if KI exists
			//		if (unit.Army == _myArmy && _myArmy == _offenerArmy) {
			if (unit.Army == _offenerArmy) {
				ShowReachableFields(unit);
				_possibleTargets = FindPossibleTargets(unit);
				//	_defenderArmy.UnHighlightUnits(_possibleTargets);
				//	_defenderArmy.HighlightUnits(_possibleTargets);
			} else if (CanAttack(unit)) {
				Attack(unit);
			}
		}

		internal void HandleDeath(Unit unit) {
			_model.UpdateFreeField(_model.GetField(unit.Position), true);
		}

		public void HandleUnitMovementComplete(Unit unit) {
			_view.ShowReachableFields(_model.GetReachableFields(unit.Position, unit.GetRemainingActionPoints()));
			_possibleTargets = FindPossibleTargets(unit);
		}

		private void Attack(Unit defender) {
			//_offenerArmy.Attack(defender);
			Unit offener = _offenerArmy.GetActiveUnit();
			offener.Fire(defender.RealPosition);
			defender.DecreaseHealth(offener.Damage);
			ShowReachableFields(offener);
		}

		private void ShowReachableFields(Unit unit) {
			_view.DestroyReachableFields();
			_view.ShowReachableFields(_model.GetReachableFields(unit.Position, unit.GetRemainingActionPoints()));
		}

		private List<Unit> FindPossibleTargets(Unit unit) {
			return new Raycaster().FindPossibleTargets(unit, _defenderArmy.GetUnits());
		}

		private bool CanAttack(Unit unit) {
			return _offenerArmy.GetActiveUnit() != null
			&& _offenerArmy.GetActiveUnit().GetRemainingActionPoints() > 0
			&& _possibleTargets.Contains(unit);
		}

		public void HandleTargetFieldSelected(Position position) {
			Field[] way = _model.GetWay(position);
			_model.UpdateFreeFields(way);
			_view.DestroyReachableFields();
			// good for debug
			//_view.ShowReachableFields(way);
			// TODO re-enable if KI exists... OR NOT?
			//			_myArmy.MoveActiveUnit(way);
			_offenerArmy.MoveActiveUnit(way);
		}

		public void OnNextTurn() {
			_view.DestroyReachableFields();
			_offenerArmy.ResetActionPoints();
			//			DisableArmy(_offenerArmy);
			EnableNextArmy();
		}

		public void OnExit() {
			SceneManager.LoadScene("Main Menu");
		}
		public void OnNextUnit() {
			Debug.Log("Next");
		}

		private void EnableNextArmy() {
			if (_offenerArmy == _myArmy) {
				_offenerArmy = _enemyArmy;
				_defenderArmy = _myArmy;
			} else {
				_offenerArmy = _myArmy;
				_defenderArmy = _enemyArmy;
			}
		}


		/*------------------------------------------------------------------------------------------*/
		/*-------------------------------------- Initilize -----------------------------------------*/
		/*------------------------------------------------------------------------------------------*/

		private void AddUnits() {
			GameObject[] spawns = GameObject.FindGameObjectsWithTag("Respawn");
			GameObject[] enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
			InitUnits(out _myArmy, spawns);
			InitUnits(out _enemyArmy, enemySpawns);
			_offenerArmy = _myArmy;
			_defenderArmy = _enemyArmy;
		}


		private void InitUnits(out Army army, GameObject[] spawns) {
			int count = spawns.Length;
			List<Unit> units = new List<Unit>(count);
			army = new Army(this, units);
			for (int i = 0; i < count; i++) {
				Vector3 localPosition = spawns[i].transform.localPosition;
				var position = _model.ConvertCoordinateToPosition(localPosition);
				Field field = _model.GetField(position);
				GameObject go = _view.AddUnit(_factory.tank, field.RealPosition);
				Unit unit = new Tank(go, army, position, localPosition);
				units.Add(unit);
				_model.UpdateAddedUnit(unit, position);
			}
		}

	}

}