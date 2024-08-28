using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public GameObject TitleMenu;
    public GameObject SettingsMenu;

    private void Start() {
        ShowTitleMenu();
        Anomaly.anomalyCount = 0;
    }

    public void ShowTitleMenu() {
        TitleMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void ShowSettingsMenu(){
        TitleMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void SwitchScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame() {
        Application.Quit();
    }

}
