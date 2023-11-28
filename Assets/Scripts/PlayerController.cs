using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private bool touching = false;

    //Death Management
    private bool dead = false;
    private float deathHeight = -30;

    //Jump Management
    private float jumpForce = 12.9f;   //Applied once at jump start
    private float jumpHoldModifier = .75f;   //Constantly applied as held
    private int jumpHoldCounter = 0;
    private int jumpHoldHeadHit = 75;
    private float jumpHoldDiminish = .01f;
    private bool isSlamming = false;
    private float slamSpeed = -23f;
    private float maxVelocity = -20f;

    //Move Management
    private float moveSpeed = 5.5f;
    private float moveSpring = 0;
    private bool moving = true;
    private float boxColliderSubtract = .05f;
    private Vector3 RightCheckBoxCollider;  //Prevent stutter when landing on ground from height by using slightly smaller box collider for wall check
    private bool onConveyor = false;
    private float conveyorSpeed = 0;

    //Grounded Checks
    private bool isGrounded = false;
    private float groundedHeight = .1f;
    private LayerMask groundLayerMask;

    //Scoring
    public TextMeshProUGUI ScoreText;
    private int score = 0;
    private float scoreCounter = 0;
    private int wormValue = 5;
    private int machineValue = 25;
    private float speedMult = .001f;

    //Destruction Effects
    public GameObject rockBurstPrefab;
    public GameObject machineBurstPrefab;
    public GameObject wormBurstPrefab;

    //Transition
    public GameObject transitionPrefab;

    //Components
    private Rigidbody2D rigi;
    private BoxCollider2D boxCollider;
    private TerrainController terrainController;
    private Animator anim;

    //SFX
    private float volume;
    private AudioSource audioSource;
    public AudioClip[] jumpSFX;
    public AudioClip[] slamSFX;
    public AudioClip deathSFX;
    public AudioClip walkingSFX;    //Add timer for consistent walking sounds while grounded?
    public AudioClip[] springSFX;
    public AudioClip[] collectSFX;
    public AudioClip[] breakableSFX;
    public AudioClip[] machineSFX;
    public AudioClip switchGoodSFX;
    public AudioClip switchBadSFX;

    void Start()
    {
        //Get SFX Audio Volume from SaveManager setting
        volume = .5f;
        audioSource = GetComponent<AudioSource>();

        rigi = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        groundLayerMask = LayerMask.GetMask("Ground");
        //terrainController = GameObject.Find("Terrain Controller").GetComponent<TerrainController>();

        RightCheckBoxCollider = new Vector3(boxCollider.bounds.size.x - boxColliderSubtract, 
            boxCollider.bounds.size.y - boxColliderSubtract, 
            boxCollider.bounds.size.z - boxColliderSubtract);
    }

    void Update()
    {
        if (moving){
            scoreCounter += Time.deltaTime;
            if (scoreCounter > 1){
                score++;

                UpdateScore(0);

                scoreCounter = 0;
                Time.timeScale = 1 + (speedMult * score);
            }
        }

        if (transform.position.y < deathHeight){
            Die();
        }
    }

    private void FixedUpdate() {
        if (!dead){
            if (CheckGrounded()){
                isGrounded = true;
                isSlamming = false;
                anim.SetBool("Airborne", false);
                anim.SetBool("Rising", false);
                anim.SetBool("Dashing", false);
                jumpHoldCounter = 0;
                moveSpring = 0;
            } else {
                anim.SetBool("Airborne", true);
                if (rigi.velocity.y > 0){
                    anim.SetBool("Rising", true);
                } else {
                    anim.SetBool("Rising", false);
                }
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
                if (rigi.velocity.y < maxVelocity){  //??? Don't understand this
                    rigi.velocity = new Vector2(rigi.velocity.x, maxVelocity);
                }
            }

            if (moving){
                if (moveSpring != 0){
                    rigi.velocity = new Vector2(moveSpeed + moveSpring, rigi.velocity.y);
                } else if (onConveyor) {
                    rigi.velocity = new Vector2(moveSpeed + conveyorSpeed, rigi.velocity.y);
                } else {
                    rigi.velocity = new Vector2(moveSpeed, rigi.velocity.y);
                }
            } else {
                rigi.velocity = new Vector2(0, rigi.velocity.y);
            }
        }
    }

    private void Die(){
        Time.timeScale = 1;
        PlayAudio(deathSFX, false, 0);
        SceneManager.LoadScene("GameOver");
    }

    private void UpdateScore(int valuePopup){
        if (valuePopup > 0){
            //Have popup showing new value added for visual flair & clarity
        }

        ScoreText.text = "Score: " + score.ToString("D4");
    }

    private void TouchStart(){
        touching = true;
        if (isGrounded){
            PlayAudio(jumpSFX, false, 0);
            ApplyJump(jumpForce);
        } else {
            PlayAudio(slamSFX, false, -.2f);
            isSlamming = true;
            anim.SetBool("Dashing", true);
        }
    }

    private void ApplyJump(float launchForce){
        rigi.velocity = new Vector2(rigi.velocity.x, launchForce);
    }

    private void TouchHeld(){
        if (CheckHead()){
            jumpHoldCounter = jumpHoldHeadHit;
        }

        rigi.velocity = new Vector2(rigi.velocity.x, rigi.velocity.y + (jumpHoldModifier - (jumpHoldCounter * jumpHoldDiminish)));

        jumpHoldCounter++;
    }

    private bool CheckGrounded(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundedHeight, groundLayerMask);

        if (raycastHit.collider != null){
            if (raycastHit.transform.tag == "Switch"){
                raycastHit.transform.parent.gameObject.GetComponent<SwitchController>().Pressed();
            } else if (raycastHit.transform.tag == "One-Way"){
                if (rigi.velocity.y >= 0.1f){
                    return false;
                }
            } else if (isSlamming && (raycastHit.transform.tag == "Breakable" || raycastHit.transform.tag == "Machine")){
                raycastHit.transform.gameObject.SetActive(false);
                if (raycastHit.transform.tag == "Machine"){
                    score += machineValue;
                    UpdateScore(machineValue);
                    PlayAudio(machineSFX, true, .1f);
                    //Add Machine Destruction Effect
                } else {
                    PlayAudio(breakableSFX, true, .1f);
                    Instantiate(rockBurstPrefab, transform.position, Quaternion.identity);
                }
                return false;   //Don't Ground player after breaking through breakables
            }

            return true;
        } else {
            return false;
        }
    }

    private bool CheckHead(){   //See if headbonk happens to stop additional height gain
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.up, groundedHeight, groundLayerMask);

        if (raycastHit.collider != null){
            return true;
        }
        return false;
    }

    private bool CheckWall(){
        RaycastHit2D rightRaycastHit = Physics2D.BoxCast(boxCollider.bounds.center, RightCheckBoxCollider, 0f, Vector2.right, groundedHeight, groundLayerMask);

        if (rightRaycastHit.collider != null && rightRaycastHit.transform.tag != "One-Way"){
            if (isGrounded){
                anim.SetBool("Pushing", true);
            } else {
                anim.SetBool("Pushing", false);
            }
            return true;
        } else {
            anim.SetBool("Pushing", false);
            return false;
        }   
    }

    public void Touch(InputAction.CallbackContext context){
        //Debug.Log("Touch Detected: " + context.phase);
        if (context.ReadValue<UnityEngine.InputSystem.TouchPhase>() == UnityEngine.InputSystem.TouchPhase.Began){
            if (touching == false){
                TouchStart();
            }
        } else if (context.ReadValue<UnityEngine.InputSystem.TouchPhase>() == UnityEngine.InputSystem.TouchPhase.Ended){
            touching = false;
        }
    }

    public void TouchBackup(InputAction.CallbackContext context){   //Keyboard & Gamepad alternatives to Touchscreen
        //Debug.Log("Backup Touch Detected: " + context.phase);
        if (context.phase == InputActionPhase.Started){
            if (touching == false){
                TouchStart();
            }
        } else if (context.phase == InputActionPhase.Canceled){
            touching = false;
        }
    }

    public void PlayAudio(AudioClip audioClip, bool clipAtPoint, float volumeModifier){
        if (audioClip != null){
            if (clipAtPoint){
                AudioSource.PlayClipAtPoint(audioClip, transform.position, volume + volumeModifier);
            } else {
                audioSource.volume = volume + volumeModifier;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }
    }

    public void PlayAudio(AudioClip[] audioClips, bool clipAtPoint, float volumeModifier){    //Overload function for choosing random sound effect from array to play
        if (audioClips.Length != 0){
            int rand = Random.Range(0, audioClips.Length-1);

            if (clipAtPoint){
                AudioSource.PlayClipAtPoint(audioClips[rand], transform.position, volume + volumeModifier);
            } else {
                audioSource.volume = volume + volumeModifier;
                audioSource.clip = audioClips[rand];
                audioSource.Play();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Worm"){
            score += wormValue;
            PlayAudio(collectSFX, true, 5f);
            UpdateScore(wormValue);
            Instantiate(wormBurstPrefab, other.transform.position, wormBurstPrefab.transform.rotation);
            other.gameObject.SetActive(false);
        } else if (other.tag == "Hurt"){
            Die();
        } else if (other.tag == "Spring"){
            Vector2 Forces = other.GetComponent<Spring>().LaunchSpring();

            isSlamming = false;
            anim.SetBool("Dashing", false);
            PlayAudio(springSFX, true, 0);

            ApplyJump(Forces.y);

            if (Forces.x != 0){
                moveSpring = Forces.x;
            }

            //Should add Anti-Slam prevention after very start of spring launch? Small counter to prevent launch momemtum accidently being stopped?
        } else if (other.tag == "End"){
            GameObject TransitionObj = Instantiate(transitionPrefab, new Vector3(transform.position.x + 10, transform.position.y, transform.position.z), Quaternion.identity);
            TransitionObj.GetComponent<Transition>().SetValues("Title", transform.position.x - 3f);
        } else if (other.tag == "Conveyor"){
            conveyorSpeed = other.GetComponent<Conveyor>().xSpeed;
            onConveyor = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.tag == "Conveyor"){
            onConveyor = false;
        }
    }
}