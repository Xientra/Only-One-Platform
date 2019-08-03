using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    public bool active = true;

    public Platform platform;
    public GameObject platformPrefab;
    public Color cursorPlatformColor;
    private Platform cursorPlatform;

    void Start() {
        platform = GameController.activeInstance.activePlatform;

        cursorPlatform = Instantiate(platformPrefab).GetComponent<Platform>();
        cursorPlatform.sprRenderer.color = cursorPlatformColor;
        cursorPlatform.col.enabled = false;
    }

    void Update() {
        active = GameController.activeInstance.gameIsRunning && GameController.activeInstance.inMainMenu == false;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        cursorPlatform.transform.position = mousePos;
        cursorPlatform.gameObject.SetActive(active);

        if (active == true && GameController.activeInstance.inMainMenu == false) {
            if (Input.GetAxis("Move Platform") != 0) {
                platform.transform.position = mousePos;
            }
        }
    }

    private void OnDestroy() {
        if (cursorPlatform != null) {
            cursorPlatform.gameObject.SetActive(false);
        }
    }
}