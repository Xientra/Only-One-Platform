using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    [HideInInspector]
    public static GameController activeInstance;
    public bool gameIsRunning = true;

    public bool[] levelProgress;

    //win if player in goal
    //lose if player in dead zone

    public PlayerController activePlayer;
    public Platform activePlatform;
    public GameObject PlatformControllerPrefab;
    public PlatformController activePlatformController;

    private Vector3 playerStart;
    private Vector3 platformStart;

    public GameObject goal;
    private GameObject goalOuterSquare;
    public float deathZoneDepth;
    private const float TIME_BEFORE_SCENE_LOAD = 2f;
    private bool hitGoal = false;

    [Space(5)]

    [HideInInspector]
    public bool inMainMenu = false;
    public GameObject spawnPoint;
    public GameObject inGameMenu;

    [Header("Effects: ")]

    public GameObject playerDeathEffect;
    public GameObject winEffect;


    void Awake() {
        if (activeInstance == null) {
            activeInstance = this;

            Load();
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        inMainMenu = (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main Menu"));

        playerStart = activePlayer.transform.position;
        platformStart = activePlatform.transform.position;

        if (goal != null) goalOuterSquare = goal.transform.GetChild(0).gameObject;
        if (activePlatformController == null) {
            activePlatformController = Instantiate(PlatformControllerPrefab).GetComponent<PlatformController>();
        }
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
        RaycastHit2D[] boxHit = Physics2D.BoxCastAll(goal.transform.position, goal.transform.lossyScale * 1.5f, 0, Vector2.zero);

        foreach (RaycastHit2D hit in boxHit) {
            if (hit.collider != null) {
                if (hit.collider.CompareTag("Player") && hitGoal == false) {
                    if (SceneManager.GetActiveScene().buildIndex + 1 - 1 < levelProgress.Length) {
                        levelProgress[SceneManager.GetActiveScene().buildIndex + 1 - 1] = true;
                    }
                    GameObject _fx = Instantiate(winEffect, goal.transform.position, Quaternion.identity, null);
                    Destroy(_fx, 1.5f);
                    StartCoroutine(LoadMainMenuAfterTime(TIME_BEFORE_SCENE_LOAD));
                    hitGoal = true;
                }
            }
        }
    }

    private void CheckDeath() {        
        if (activePlayer.transform.position.y < deathZoneDepth) {
            GameObject _fx = Instantiate(playerDeathEffect, activePlayer.transform.position, Quaternion.identity, null);
            Destroy(_fx, 1f);
            gameIsRunning = false;
            StartCoroutine(RespawnPlayer());
        }
    }

    private IEnumerator RespawnPlayer() {
        activePlayer.gameObject.SetActive(false);
        yield return new WaitForSeconds(TIME_BEFORE_SCENE_LOAD);
        activePlayer.gameObject.SetActive(true);
        activePlayer.transform.position = playerStart;
        activePlatform.transform.position = platformStart;
        activePlatformController.AdjustPlatform();
        gameIsRunning = true;
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
        Save();
    }
    private IEnumerator LoadMainMenuAfterTime(float time) {
        yield return new WaitForSeconds(time);
        if (activePlayer != null) {
            LoadMainMenu();
        }
    }

    public void OnDestroy() {
        Save();
    }

    private string LEVEL_SAVE_NAME = "level";
    public void Save() {
        for (int i = 0; i < levelProgress.Length; i++) {
            PlayerPrefs.SetString(LEVEL_SAVE_NAME + i.ToString(), levelProgress[i].ToString());
        }
        PlayerPrefs.Save();
    }

    public void Load() {
        for (int i = 0; i < levelProgress.Length; i++) {
            levelProgress[i] = PlayerPrefs.GetString(LEVEL_SAVE_NAME + i.ToString()).ToLower() == true.ToString().ToLower();
        }
    }
}