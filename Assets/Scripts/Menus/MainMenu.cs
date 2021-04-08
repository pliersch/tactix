using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  public Button _playButton;

  // Start is called before the first frame update
  void Start() {
    _playButton.onClick.AddListener(OnPlay);
  }

  // Update is called once per frame
  void Update() {

  }

  private void OnPlay() {
    SceneManager.LoadScene("Battlefield");
  }
}
