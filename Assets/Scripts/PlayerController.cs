using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private const float RAY_DISTANCE = 0.1f;
    public bool removeControll = false;
    public bool pauseMovement = false;

    public float movementSpeed = 10f;

    [Space(10)]

    public float gravityStrength = 10f;
    private float gravityForce;
    public float gravityForceChangeSpeed = 0.2f;

    [Space(10)]

    public float jumpStrength = 1f;
    private float jumpForce;
    public float jumpForceChangeSpeed = 0.1f;
    public float lowjumpHeight = 0.25f; //in percent of normal jump height

    [Space(10)]

    public float isGroundedDistance = 0.1f;
    public float coyoteTime = 0.05f;

    [Space(10)]

    float horizontalSpeed;
    bool isGrounded = false;
    //bool canWallJump = false;
    bool jumping = false;
    //bool walljumping = false;


    void Start() {

    }

    void Update() {
        isGrounded = CheckIfGrounded();

        if (pauseMovement == false) {
            if (removeControll == false) {
                MovementInput();
            }
            PerformMovment();
        }
    }

    private void MovementInput() {
        horizontalSpeed = Input.GetAxis("Horizontal");

        if (isGrounded == true && Input.GetAxis("Jump") != 0) {
            jumping = true;
            jumpForce = jumpStrength;
        }
    }

    private void PerformMovment() {
        if (horizontalSpeed != 0) {
            MoveHorizontal(horizontalSpeed * movementSpeed);
        }

        if (jumping == true) {
            float lowJumpMulti = 0f;
            if (Input.GetAxis("Jump") != 0) {
                lowJumpMulti = 1f;
            }
            else lowJumpMulti = (1 / lowjumpHeight);

            jumpForce -= jumpStrength * jumpForceChangeSpeed * lowJumpMulti * Time.deltaTime;
            if (jumpForce < 0) {
                jumping = false;
            }

            MoveVertical(jumpForce);
        }
        else {
            if (gravityForce < gravityStrength) {
                gravityForce += gravityStrength * gravityForceChangeSpeed * Time.deltaTime;
            }
            else gravityForce = gravityStrength;
            MoveVertical(-gravityForce);
        }

        if (isGrounded == true) {
            gravityForce = 0f;
        }
    }

    bool CheckIfGrounded() {
        bool result = false;

        for (float d = -coyoteTime; d <= transform.lossyScale.x + coyoteTime; d += RAY_DISTANCE) {
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position + new Vector3(-(transform.lossyScale.x / 2) + d, -transform.lossyScale.y / 2), Vector3.up, -isGroundedDistance);
            if (rayHit.collider != null) {
                if (rayHit.collider.gameObject.layer == 8) { //Layer 8 is "ground"
                    result = true;
                }
            }
            Debug.DrawRay(transform.position + new Vector3(-(transform.lossyScale.x / 2) + d, -transform.lossyScale.y / 2), Vector3.up * -isGroundedDistance, Color.red);
        }

        return result;
    }

    bool MoveHorizontal(float translation) {
        float toMoveDistance = translation * Time.deltaTime;
        int direction = (int)Mathf.Sign(translation); //is either 1 or -1
        bool hitSth = false;

        for (float d = RAY_DISTANCE * 0.5f; d <= transform.lossyScale.y - RAY_DISTANCE * 0.5f; d += RAY_DISTANCE) {

            RaycastHit2D rayHit = Physics2D.Raycast(transform.position + new Vector3(direction * transform.lossyScale.x / 2, -(transform.lossyScale.y / 2) + d), Vector3.right, toMoveDistance);

            if (rayHit.collider != null) {
                if (rayHit.collider.isTrigger != true) {
                    toMoveDistance = direction * (-direction * (transform.position.x + (direction * transform.lossyScale.x / 2)) + (direction * rayHit.point.x)); //this now will allways take the last ray, if that one hit
                    hitSth = true;
                }
            }

            Debug.DrawRay(transform.position + new Vector3(direction * transform.lossyScale.x / 2, -(transform.lossyScale.y / 2) + d), Vector3.right * toMoveDistance, Color.green);
        }

        transform.position += Vector3.right * toMoveDistance;
        return hitSth;
    }

    bool MoveVertical(float translation) {
        return MoveVertical(translation, true);
    }
    bool MoveVertical(float translation, bool performMovement) {
        float toMoveDistance = translation * Time.deltaTime;
        int direction = (int)Mathf.Sign(translation); //is either 1 or -1
        bool hitSth = false;

        for (float d = RAY_DISTANCE * 0.5f; d <= transform.lossyScale.x - RAY_DISTANCE * 0.5f; d += RAY_DISTANCE) {
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position + new Vector3(-(transform.lossyScale.x / 2) + d, direction * transform.lossyScale.y / 2), Vector3.up, toMoveDistance);
            if (rayHit.collider != null) {
                if (rayHit.collider.isTrigger != true) {
                    toMoveDistance = direction * (-direction * (transform.position.y + (direction * transform.lossyScale.y / 2)) + (direction * rayHit.point.y)); //this now will allways take the last ray, if that one hit
                    hitSth = true;
                }
            }

            Debug.DrawRay(transform.position + new Vector3(-(transform.lossyScale.x / 2) + d, direction * transform.lossyScale.y / 2), Vector3.up * toMoveDistance, Color.blue);
        }

        if (performMovement == true) transform.position += Vector3.up * toMoveDistance;
        return hitSth;
    }
}