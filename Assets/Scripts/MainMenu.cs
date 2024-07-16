using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button[] gameButtons;
    [SerializeField] private GameObject settingsScreen;
    private bool settingsActive = false;

    public void LoadSceneAt(int index) {
      SceneManager.LoadScene(index);
    }

    public void ToggleSettingsScreen() {
      if (!settingsActive) { settingsScreen.SetActive(true); settingsActive = true; }
      else { settingsScreen.SetActive(false); settingsActive = false; }
    }
}
