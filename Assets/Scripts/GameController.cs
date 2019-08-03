using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    [HideInInspector]
    public static GameController activeInstance;
    public bool gameIsRunning = true;

    public bool[] levelProgress = { true, false };

    //win if player in goal
    //lose if player in dead zone

    public PlayerController activePlayer;
    public Platform activePlatform;

    public GameObject goal;
    private GameObject goalOuterSquare;
    public float deathZoneDepth;
    private const float TIME_BEFORE_SCENE_LOAD = 2f;

    [Space(5)]

    [HideInInspector]
    public bool inMainMenu = false;
    public GameObject spawnPoint;
    public GameObject inGameMenu;

    [Header("Effects: ")]

    public GameObject playerDeathEffect;


    void Awake() {
        if (activeInstance == null) {
            activeInstance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        if (goal != null) goalOuterSquare = goal.transform.GetChild(0).gameObject;
        inMainMenu = (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main Menu"));
    }

    void Update() {

        if (inMainMenu == false) {
            if (Input.GetKeyDown(KeyCode.Escape)) {                
                OpenInGameMenu(!inGameMenu.activeSelf);
            }
        }

        if (gameIsRunning == true) {
            if (inMainMenu == false) CheckWin();
            CheckDeath();
        }
        AnimateGoal();
    }

    private void CheckWin() {
        RaycastHit2D[] boxHit = Physics2D.BoxCastAll(goal.transform.position, goal.transform.lossyScale * 1.5f, 0, Vector2.up);

        foreach (RaycastHit2D hit in boxHit) {
            if (hit.collider != null) {
                if (hit.collider.CompareTag("Player")) {
                    if (SceneManager.GetActiveScene().buildIndex + 1 > levelProgress.Length) {
                        levelProgress[SceneManager.GetActiveScene().buildIndex - 1] = true;
                        StartCoroutine(LoadMainMenuAfterTime(TIME_BEFORE_SCENE_LOAD));
                    }
                }
            }
        }
    }

    private void CheckDeath() {        
        if (activePlayer.transform.position.y < deathZoneDepth) {
            GameObject _fx = Instantiate(playerDeathEffect, activePlayer.transform.position, Quaternion.identity, null);
            Destroy(_fx, 1f);
            if (inMainMenu) {
                activePlayer.transform.position = spawnPoint.transform.position;
            }
            else {
                Destroy(activePlayer.gameObject);
                gameIsRunning = false;
                StartCoroutine(ReloadScene());
            }
        }
    }

    private IEnumerator ReloadScene() {
        yield return new WaitForSeconds(TIME_BEFORE_SCENE_LOAD);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void AnimateGoal() {
        if (goal != null) {
            goal.transform.Rotate(Vector3.forward, -0.1f);
            goalOuterSquare.transform.Rotate(Vector3.forward, 0.25f);
        }
    }

    public void OpenInGameMenu(bool value) {
        inGameMenu.SetActive(value);
        gameIsRunning = !value;
        activePlayer.pauseMovement = value;
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }
    private IEnumerator LoadMainMenuAfterTime(float time) {
        yield return new WaitForSeconds(time);
        LoadMainMenu();
    }
}