﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

    public enum ZoneType { NoPlaceZone, ShrinkZone, WallZone, TurningZone, DiscreteZone}
    public ZoneType type = ZoneType.NoPlaceZone;

    public GameObject discretePlatformPosition;

    [Header("Animation: ")]
    public Transform[] lines;
    public bool stopAnimation = false;
    public float animationSpeed = 1f; //animationDistance travelled in this time
    public float animationDistance = 0.12f;
    private const float ANIMATION_PERCENT_STEP = 0.01f;

    void Start() {
        foreach (Transform t in lines) {
            StartCoroutine(Animate(t));
        }
    }

    void Update() {
        if (discretePlatformPosition != null) {
            discretePlatformPosition.GetComponent<SpriteRenderer>().size = (GameController.activeInstance.activePlatformController.transform.localScale) + new Vector3(0.2f, 0.2f);
        }
    }


    private IEnumerator Animate(Transform toAnimate) {
        while (stopAnimation == false) {
            yield return new WaitForSeconds((animationDistance * ANIMATION_PERCENT_STEP) * animationSpeed);
            //toAnimate.transform.Translate(animationDistance * ANIMATION_PERCENT_STEP, 0, 0, toAnimate.transform);
            toAnimate.transform.localPosition += toAnimate.transform.right * animationDistance * ANIMATION_PERCENT_STEP;

            if (toAnimate.transform.localPosition.magnitude >= animationDistance) {
                toAnimate.transform.localPosition = Vector2.zero;
            }
        }
    }
}