using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private bool touching = false;

    //Death Management
    private bool dead = false;
    private float deathHeight = -30;

    //Jump Management
    private float jumpForce = 13f;   //Applied once at jump start
    private float jumpHoldModifier = .75f;   //Constantly applied as held
    private int jumpHoldCounter = 0;
    private float jumpHoldDiminish = .01f;
    private bool isSlamming = false;
    private float slamSpeed = -23f;
    private float maxVelocity = -20f;

    //Move Management
    private float moveSpeed = 5.5f;
    private bool moving = true;
    private float boxColliderSubtract = .05f;
    private Vector3 RightCheckBoxCollider;  //Prevent stutter when landing on ground from height by using slightly smaller box collider for wall check

    //Grounded Checks
    private bool isGrounded = false;
    private float groundedHeight = .1f;
    private LayerMask groundLayerMask;

    //Components
    private Rigidbody2D rigi;
    private BoxCollider2D boxCollider;
    private TerrainController terrainController;

    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
        terrainController = GameObject.Find("Terrain Controller").GetComponent<TerrainController>();

        RightCheckBoxCollider = new Vector3(boxCollider.bounds.size.x - boxColliderSubtract, 
            boxCollider.bounds.size.y - boxColliderSubtract, 
            boxCollider.bounds.size.z - boxColliderSubtract);
    }

    void Update()
    {
        if (moving){
            transform.position = new Vector3(transform.position.x + (moveSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }

        if (transform.position.y < deathHeight){
            SceneManager.LoadScene("GameOver");
        }
    }

    private void FixedUpdate() {
        if (!dead){
            if (CheckGrounded()){
                isGrounded = true;
                isSlamming = false;

                jumpHoldCounter = 0;
            } else {
                isGrounded = false;
            }

            if (CheckWall()){
                moving = false;
            } else {
                if (!isSlamming){
                    moving = true;
                }
            }

            if (touching && !isSlamming){
                TouchHeld();
            }

            if (isSlamming){
                moving = false;
                rigi.velocity = new Vector2(rigi.velocity.x, slamSpeed);
            } else {
                if (rigi.velocity.y < maxVelocity){
                    rigi.velocity = new Vector2(rigi.velocity.x, maxVelocity);
                }
            }
        }
    }

    private void TouchStart(){
        touching = true;
        if (isGrounded){
            ApplyJump(jumpForce);
        } else {
            isSlamming = true;
        }
    }

    private void ApplyJump(float launchForce){
        rigi.velocity = new Vector2(rigi.velocity.x, launchForce);
    }

    private void TouchHeld(){
        rigi.velocity = new Vector2(rigi.velocity.x, rigi.velocity.y + (jumpHoldModifier - (jumpHoldCounter * jumpHoldDiminish)));

        jumpHoldCounter++;
    }

    private bool CheckGrounded(){
        if (rigi.velocity.y <= 0.1f && rigi.velocity.y > -.1f){  //Prevent rising grounded state through semi-solid platforms
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundedHeight, groundLayerMask);
            
            return raycastHit.collider != null;
        } else {
            return false;
        }
    }

    private bool CheckWall(){
        RaycastHit2D rightRaycastHit = Physics2D.BoxCast(boxCollider.bounds.center, RightCheckBoxCollider, 0f, Vector2.right, groundedHeight, groundLayerMask);

        if (rightRaycastHit.collider != null){
            return true;
        } else {
            return false;
        }   
    }

    public void Touch(InputAction.CallbackContext context){
        Debug.Log("Touch Detected: " + context.phase);

        if (context.ReadValue<UnityEngine.InputSystem.TouchPhase>() == UnityEngine.InputSystem.TouchPhase.Began){
            if (touching == false){
                TouchStart();
            }
        } else if (context.ReadValue<UnityEngine.InputSystem.TouchPhase>() == UnityEngine.InputSystem.TouchPhase.Ended){
            touching = false;
        }
    }

    public void TouchBackup(InputAction.CallbackContext context){   //Keyboard & Gamepad alternatives to Touchscreen
        Debug.Log("Backup Touch Detected: " + context.phase);
        if (context.phase == InputActionPhase.Started){
            if (touching == false){
                TouchStart();
            }
        } else if (context.phase == InputActionPhase.Canceled){
            touching = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        //Apply Hurt & Item pickup
    }
}
