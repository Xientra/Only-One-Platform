using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    public Platform platform;

    void Start() {
        
    }

    void Update() {
        if (Input.GetAxis("Move Platform") != 0) {

            Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            platform.transform.position = newPos;
        }
    }
}