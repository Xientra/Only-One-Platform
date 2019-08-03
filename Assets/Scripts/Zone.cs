using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

    bool stopAnimation = false;
    public float animationDistance = 0.2475f;
    public float animationSpeed = 1f; //animationDistance travelled in this time
    public GameObject lines;

    void Start() {
        StartCoroutine(Animate());
    }

    void Update() {

    }

    private const float ANIMATION_PERCENT_STEP = 0.05f;
    private IEnumerator Animate() {
        while (stopAnimation == false) {
            yield return new WaitForSeconds((animationDistance * ANIMATION_PERCENT_STEP) * animationSpeed);
            lines.transform.Translate(animationDistance * ANIMATION_PERCENT_STEP, 0, 0);
            if (lines.transform.localPosition.x >= animationDistance) {
                lines.transform.localPosition = Vector2.zero;
            }
        }
    }
}