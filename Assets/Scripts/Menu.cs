﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public void Btn_LoadLevel(int index) {
        SceneManager.LoadScene(index);
    }

    public void Btn_exit() {
        Application.Quit();
    }
}