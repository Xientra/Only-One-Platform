using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    public bool active = true;
    private Vector2 mousePos;

    public Platform platform;
    private SpriteRenderer spr;

    [Header("Zone Modifikations: ")]
    public Color defaulColor;
    public Color unableToPlaceColor;
    private bool unableToPlace = false;

    private Vector2 originalSize;
    public float shrunkenWidth = 1.5f;
    private bool removeNextPlatform = false;
    public float removeTime = 2f;



    void Start() {
        platform = GameController.activeInstance.activePlatform;
        spr = GetComponent<SpriteRenderer>();
        originalSize = transform.lossyScale;

        spr.color = defaulColor;
    }

    void Update() {
        active = GameController.activeInstance.gameIsRunning && GameController.activeInstance.inMainMenu == false;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        spr.enabled = active;


        if (active == true) {
            CheckForZones();
        }
        else {
            SetDefaults();
        }

        if (active == true && GameController.activeInstance.inMainMenu == false && unableToPlace == false) {
            if (Input.GetAxis("Move Platform") != 0) {
                platform.transform.position = transform.position;
                AdjustPlatform();
            }
        }
    }

    public void AdjustPlatform() {
        platform.gameObject.SetActive(true);
        platform.transform.localScale = transform.localScale;
        platform.transform.rotation = transform.rotation;

        if (removeNextPlatform == true) {
            StartCoroutine(RemovePlatform(removeTime, mousePos));
        }
    }

    private void CheckForZones() {
        Vector2 boxCollider = new Vector2(1f, 0.5f);
        RaycastHit2D[] allHits = Physics2D.BoxCastAll(mousePos, boxCollider , 0, Vector2.zero);

        SetDefaults();

        foreach (RaycastHit2D hit in allHits) {
            Zone collidedZone = hit.collider.GetComponent<Zone>();
            if (collidedZone != null) {
                Debug.Log("collided with " + collidedZone.type);
                switch (collidedZone.type) {
                    case (Zone.ZoneType.NoPlaceZone):
                        spr.color = unableToPlaceColor;
                        unableToPlace = true;
                        break;
                    case (Zone.ZoneType.WallZone):
                        transform.localScale = new Vector2(transform.localScale.y, transform.localScale.x);
                        break;
                    case (Zone.ZoneType.ShrinkZone):
                        if (transform.localScale.x > transform.localScale.y) {
                            transform.localScale = new Vector3(shrunkenWidth, transform.localScale.y);
                        }
                        else {
                            transform.localScale = new Vector3(transform.localScale.x, shrunkenWidth);
                        }
                        break;
                    case (Zone.ZoneType.RemoveZone):
                        removeNextPlatform = true;
                        break;
                    case (Zone.ZoneType.DiscreteZone):
                        transform.position = collidedZone.discretePlatformPosition.transform.position;
                        break;
                }
            }
        }
    }


    public void SetDefaults() {
        //NoPlace
        spr.color = defaulColor;
        unableToPlace = false;
        //Shrink
        transform.localScale = originalSize;
        //Wall
        transform.rotation = Quaternion.identity;
        //discrete
        transform.position = mousePos;
        //remove
        removeNextPlatform = false;
    }

    private IEnumerator RemovePlatform(float delay, Vector3 position) {
        yield return new WaitForSeconds(delay);
        if (platform.transform.position == position) {
            platform.gameObject.SetActive(false);
        }
    }
}