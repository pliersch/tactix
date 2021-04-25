using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Battlefield.Map {

	public class MapUI : MonoBehaviour {

		public Map map;
		public Button nextTurnButton;
		public Button exitButton;
		public Button quitAppButton;
		public Button nextUnitButton;
		public QuitApplication quitApplication;

		private void Start() {
			nextTurnButton.onClick.AddListener(OnNextTurn);
			exitButton.onClick.AddListener(OnExit);
			quitAppButton.onClick.AddListener(OnQuitGame);
			nextUnitButton.onClick.AddListener(OnNextUnit);
		}

		private void OnNextTurn() {
			map.HandleNextTurn();
		}

		private void OnNextUnit() {
			map.HandleNextUnit();
		}

		private void OnExit() {
			SceneManager.LoadScene("Main Menu");
		}

		private void OnQuitGame() {
			quitApplication.Quit();
		}

	}

}