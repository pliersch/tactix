using System.Collections.Generic;
using Game.Battlefield.Tanks;
using Game.Battlefield.util;
using Game.Units;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Battlefield.Map {

	public class Map : MonoBehaviour, IBattlefieldViewController {

		private MapModel _model;
		public MapView _view;
		public PrefabFactory _factory;
		public LevelChecker _levelChecker;
		public QuitApplication _quitApplication;
		public Button _nextTurnButton;
		public Button _exitButton;
		public Button _quitAppButton;
		public Button _nextUnitButton;
		public int _rows;
		public int _columns;
		private Army _myArmy;
		private Army _enemyArmy;
		private Army _attackerArmy;
		private Army _defenderArmy;
		private List<Unit> _possibleTargets;

		// TODO: better Use Unity Editor scripts and check it before compile (no code in final game)
		private void Start() {
			_nextTurnButton.onClick.AddListener(OnNextTurn);
			_exitButton.onClick.AddListener(OnExit);
			_quitAppButton.onClick.AddListener(OnQuitGame);
			_nextUnitButton.onClick.AddListener(OnNextUnit);
			// AddUnits();
			init();
		}

		private void init() {
			_model = new MapModel();
			float tileSize = _levelChecker.CheckTileSize(_factory.tile);
			Field[,] fields = FieldFactory.GenerateFields(_rows, _columns, tileSize);
			_model.SetTileSize(tileSize);
			_model.SetFields(fields);
			_levelChecker.FindOccupiedFields(fields, _factory);
			_view.SetController(this);
			Army[] armies = UnitFactory.InitArmies(2, 4);
			_myArmy = armies[0];
			_enemyArmy = armies[1];
			_myArmy.Map = this;
			_enemyArmy.Map = this;
			_attackerArmy = _myArmy;
			_defenderArmy = _enemyArmy;
			GameObject[] spawns = GameObject.FindGameObjectsWithTag("Respawn");
			GameObject[] enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
			InitUnits(ref _myArmy, spawns);
			InitUnits(ref _enemyArmy, enemySpawns);
		}
		
		private void InitUnits(ref Army army, GameObject[] spawns) {
			int count = army.GetUnits().Count;
			for (int i = 0; i < count; i++) {
				Vector3 localPosition = spawns[i].transform.localPosition;
				Position position = _model.ConvertCoordinateToPosition(localPosition);
				Field field = _model.GetField(position);
				GameObject go = _view.AddUnit(_factory.tank, field.RealPosition);
				Unit unit = army.GetUnits()[i];
				unit.Position = position;
				unit.RealPosition = localPosition;
				unit.GameObject = go;
				TankActionHandler actionHandler = go.GetComponent<TankActionHandler>();
				actionHandler.SetInteractionHandler(unit);
				// Unit unit = new Tank(go, army, position, localPosition);
				_model.UpdateAddedUnit(unit, position);
			}
		}

		public void HandleUnitSelected(Unit unit) {
			
			// TODO re-enable if KI exists
			//		if (unit.Army == _myArmy && _myArmy == _attackerArmy) {
			if (unit.Army == _attackerArmy) {
				ShowReachableFields(unit);
				_possibleTargets = FindPossibleTargets(unit);
				ShowPossibleTargets(_possibleTargets);
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
			ShowPossibleTargets(_possibleTargets);
		}

		private void Attack(Unit defender) {
			//_attackerArmy.Attack(defender);
			Unit activeUnit = _attackerArmy.GetActiveUnit();
			activeUnit.Fire(defender.RealPosition);
			defender.DecreaseHealth(activeUnit.Damage);
			ShowReachableFields(activeUnit);
		}

		private void ShowReachableFields(Unit unit) {
			_view.DestroyReachableFields();
			_view.ShowReachableFields(_model.GetReachableFields(unit.Position, unit.GetRemainingActionPoints()));
		}

		private void ShowPossibleTargets(List<Unit> units) {
			foreach (Unit unit in _defenderArmy.GetUnits()) {
				unit.UnHighlight();
			}			
			foreach (Unit unit in units) {
				unit.Highlight();
			}
		}

		private List<Unit> FindPossibleTargets(Unit unit) {
			return Raycaster.FindPossibleTargets(unit, _defenderArmy.GetUnits());
		}

		private bool CanAttack(Unit unit) {
			return _attackerArmy.GetActiveUnit() != null
			       && _attackerArmy.GetActiveUnit().GetRemainingActionPoints() > 0
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
			_attackerArmy.MoveActiveUnit(way);
		}
		
		/*------------------------------------------------------------------------------------------*/
		/*--------------------------------------- UI Event -----------------------------------------*/
		/*------------------------------------------------------------------------------------------*/

		private void OnNextTurn() {
			_view.DestroyReachableFields();
			_attackerArmy.ResetActionPoints();
			//			DisableArmy(_attackerArmy);
			EnableNextArmy();
		}

		private void OnExit() {
			SceneManager.LoadScene("Main Menu");
		}

		private void OnNextUnit() {
			_attackerArmy.ComputeNextActiveUnit();
		}
		private void OnQuitGame() {
			_quitApplication.Quit();
		}
		
		private void EnableNextArmy() {
			if (_attackerArmy == _myArmy) {
				_attackerArmy = _enemyArmy;
				_defenderArmy = _myArmy;
			} else {
				_attackerArmy = _myArmy;
				_defenderArmy = _enemyArmy;
			}
		}
		
		// /*------------------------------------------------------------------------------------------*/
		// /*-------------------------------------- Initialize ----------------------------------------*/
		// /*------------------------------------------------------------------------------------------*/
		//
		// private void AddUnits() {
		// 	GameObject[] spawns = GameObject.FindGameObjectsWithTag("Respawn");
		// 	GameObject[] enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
		// 	InitUnits(out _myArmy, spawns);
		// 	InitUnits(out _enemyArmy, enemySpawns);
		// 	_attackerArmy = _myArmy;
		// 	_defenderArmy = _enemyArmy;
		// }
		//
		//
		// private void InitUnits(out Army army, GameObject[] spawns) {
		// 	int count = spawns.Length;
		// 	List<Unit> units = new List<Unit>(count);
		// 	army = new Army(units) {
		// 		Map = this
		// 	};
		// 	for (int i = 0; i < count; i++) {
		// 		Vector3 localPosition = spawns[i].transform.localPosition;
		// 		Position position = _model.ConvertCoordinateToPosition(localPosition);
		// 		Field field = _model.GetField(position);
		// 		GameObject go = _view.AddUnit(_factory.tank, field.RealPosition);
		// 		Unit unit = new Tank(go, army, position, localPosition);
		// 		units.Add(unit);
		// 		_model.UpdateAddedUnit(unit, position);
		// 	}
		// }

	}

}