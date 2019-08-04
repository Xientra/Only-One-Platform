using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public Button[] levelButtons;

    public void Start() {
        UpdateLevelButtons();
    }

    public void UpdateLevelButtons() {
        int i = 0;
        foreach (Button b in levelButtons) {
            b.interactable = GameController.activeInstance.levelProgress[i];
            i++;
        }
    }

    public void Btn_LoadLevel(int index) {
        SceneManager.LoadScene(index);
    }

    public void Btn_exit() {
        Application.Quit();
    }
}