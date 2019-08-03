using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Platform : MonoBehaviour {

    public Collider2D col;
    public SpriteRenderer sprRenderer;

    public void Start() {
        //col = GetComponent<Collider2D>();
        //sprRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeToGround() {
        transform.Rotate(Vector3.forward, 90);
    }

    public void ChangeToWall() {
        transform.Rotate(Vector3.forward, -90);
    }
}