using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Platform : MonoBehaviour {

    [HideInInspector]
    public Collider2D collider;

    public void Start() {
        collider = GetComponent<Collider2D>();
        
    }

    public void ChangeToGround() {
        transform.Rotate(Vector3.forward, 90);
    }

    public void ChangeToWall() {
        transform.Rotate(Vector3.forward, -90);
    }
}