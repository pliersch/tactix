using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

  public Button _playButton;

  void Start() {
    _playButton.onClick.AddListener(() => SceneManager.LoadScene("Battlefield"));
  }

}
