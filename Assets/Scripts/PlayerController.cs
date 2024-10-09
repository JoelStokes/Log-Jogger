using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private bool touching = false;

    //Death Management
    public GameObject AngelPrefab;
    public RespawnMenu respawnMenu;
    private bool dead = false;
    private float deathHeight = -25;
    private float deathTimer;
    private float deathLim = 0.75f;
    private bool angelSpawned = false;

    //Start Management
    private float startTimer;
    private float startLim = 1;
    private bool started = false;

    //Jump Management
    private float jumpForce = 12.9f;   //Applied once at jump start
    private float jumpHoldModifier = .75f;   //Constantly applied as held
    private int jumpHoldCounter = 0;
    private int jumpHoldHeadHit = 75;
    private float jumpHoldDiminish = .01f;
    private bool isSlamming = false;
    private float slamPrevY = 0;
    private float slamXAdj = .025f;  //Used as preventative measure to stop slam edge cases with frozen player
    private float slamYAdj = .005f;
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
    private bool paused = false;
    private bool pushingWall = false;
    private float ghostCollisionAdd = .005f;  //Value to add in rare ghost collision to force player out of stuck state
    private float ghostPrevX = 0;
    private float ghostPrevY = 0;

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
    private float speedMult = .00085f;
    private string sceneName;

    //Effects
    public GameObject rockBurstPrefab;
    public GameObject machineBurstPrefab;
    public GameObject wormBurstPrefab;
    public LineRenderer lineRenderer;
    private List<Vector3> prevPositions = new List<Vector3>();
    private int trailLength = 15;
    private float trailXAdj = -0.11f;
    private float trailYAdj = -0.33f;

    //Transition
    public GameObject transitionPrefab;

    //Components
    private Rigidbody2D rigi;
    public BoxCollider2D bottomCollider;
    public BoxCollider2D frontCollider;
    public CircleCollider2D frontCircle;
    public CircleCollider2D backCircle;
    private TerrainController terrainController;
    private Animator anim;
    private CameraController cameraController;
    private ChunkExit lastChunk;
    private SaveManager saveManager;
    private VolumeController volumeController;
    private Rect pauseRegion;

    //SFX
    private float volume;
    private int currentAudioSource = 0;
    private AudioSource audioSource;
    public AudioSource[] rotatingAudioSource;
    public AudioClip[] jumpSFX;
    public AudioClip[] slamSFX;
    public AudioClip deathSFX;
    public AudioClip walkingSFX;    //Add timer for consistent walking sounds while grounded?
    public AudioClip[] springSFX;
    public AudioClip[] collectSFX;
    public AudioClip[] breakableSFX;
    public AudioClip[] machineSFX;
    public AudioClip machinePointsSFX;
    private float fadeSpeed = .75f;

    //Sound Effect Modifiers
    private float jumpSFXMod = .55f;
    private float slamSFXMod = .55f;
    private float wormSFXMod = .9f;
    private float springSFXMod = 1.2f;
    private float breakableSFXMod = .45f;
    private float machineSFXMod = .5f;
    private float machinePointsSFXMod = 1.2f;
    private float deathSFXMod = 1.1f;

    //Trigger Repeat Prevention
    private bool wormTrigger = false;
    private bool springTrigger = false;

    //Special Skin Handlers
    private bool isRGB = false;
    private float redValue = 0;
    private float greenValue = 0;
    private float blueValue = 1;
    private float colorChangeSpeed = 1f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cameraController = Camera.main.gameObject.GetComponent<CameraController>();
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        pauseRegion = RectTransformToScreenSpace(GameObject.Find("Pause").GetComponent<RectTransform>(), Camera.main, false);

        rigi = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        groundLayerMask = LayerMask.GetMask("Ground");
        //terrainController = GameObject.Find("Terrain Controller").GetComponent<TerrainController>();

        RightCheckBoxCollider = new Vector3(frontCollider.bounds.size.x - boxColliderSubtract, 
            frontCollider.bounds.size.y - boxColliderSubtract, 
            frontCollider.bounds.size.z - boxColliderSubtract);

        //Used to stop speed from increasing on tutorial level
        sceneName = SceneManager.GetActiveScene().name;

        //Used for music slowdown & stop on death
        volumeController = GameObject.Find("MusicManager").GetComponent<VolumeController>();
        volume = saveManager.state.sfxVolume;

        //Set all lineRenderer values to current position
        lineRenderer.positionCount = trailLength;
        for (int i=0; i<trailLength; i++){
            prevPositions.Add(new Vector3(transform.position.x + trailXAdj, transform.position.y + trailYAdj, transform.position.z));
        }
        HandleTrail();

        if (saveManager.state.currentSkin == 12){
            isRGB = true;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Reset Triggers
        wormTrigger = false;
        springTrigger = false;

        //Handle RGB Animation
        if (isRGB && !dead){
            if (blueValue >= 1 && greenValue < 1 && redValue <= 0){
                greenValue += colorChangeSpeed * Time.deltaTime;
            } else if (greenValue >= 1 && blueValue > 0){
                blueValue -= colorChangeSpeed * Time.deltaTime;
            } else if (greenValue >= 1 && redValue < 1){
                redValue += colorChangeSpeed * Time.deltaTime;
            } else if (redValue >= 1 && greenValue > 0){
                greenValue -= colorChangeSpeed * Time.deltaTime;
            } else if (redValue >= 1 && blueValue < 1) {
                blueValue += colorChangeSpeed * Time.deltaTime;
            } else if (blueValue >= 1 && redValue > 0){
                redValue -= colorChangeSpeed * Time.deltaTime;
            }

            spriteRenderer.color = new Vector4(redValue, greenValue, blueValue, 1);
            lineRenderer.startColor = new Vector4(redValue, greenValue, blueValue, .6f);
            lineRenderer.endColor = new Vector4(redValue, greenValue, blueValue, 0);
        }

        if (moving && !dead){
            scoreCounter += Time.deltaTime;
            if (scoreCounter > 1){
                score++;

                UpdateScore(0);

                scoreCounter = 0;
                if (sceneName != "Tutorial"){
                    Time.timeScale = 1 + (speedMult * score);
                }
            }
        }

        if (transform.position.y < deathHeight){
            Die();
        }
    }

    private void FixedUpdate() {
        if (!dead && started){
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
                pushingWall = true;
            } else {
                if (!isSlamming){
                    moving = true;
                }
                pushingWall = false;
            }

            if (touching && !isSlamming){
                TouchHeld();
            } else if (!touching && !isSlamming && !isGrounded){    //Fade jump sfx
                FadeAudio();
            }

            if (isSlamming){
                moving = false;

                if (slamPrevY == transform.position.y){  //May be stuck, add very small X value to see if it dislodges player
                    transform.position = new Vector3(transform.position.x + slamXAdj, transform.position.y + slamYAdj, transform.position.z);
                }
                slamPrevY = transform.position.y;
                rigi.velocity = new Vector2(rigi.velocity.x, slamSpeed);
            } else {
                if (rigi.velocity.y < maxVelocity){  //??? Don't understand this
                    rigi.velocity = new Vector2(rigi.velocity.x, maxVelocity);
                }
                slamPrevY = -100;   //Large arbitrary value so next slam starts fresh
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
        } else if (!started){
            startTimer += Time.deltaTime;
            if (startTimer >= startLim){
                started = true;
                anim.SetTrigger("Start");
            }
        } else if (dead && !angelSpawned) {
            deathTimer += Time.deltaTime;
            if (deathTimer >= deathLim){
                Instantiate(AngelPrefab, transform.position, AngelPrefab.transform.rotation);
                GetComponent<SpriteRenderer>().color = new Vector4(1,1,1,0);

                respawnMenu.PlayerDied();

                angelSpawned = true;
            }
        }

        HandleTrail();
    }

    private void Die(){
        if (!dead){
            Time.timeScale = 1;
            PlayAudio(deathSFX, false, deathSFXMod);
            dead = true;
            anim.SetTrigger("Die");

            rigi.isKinematic = true;
            rigi.velocity = Vector2.zero;

            frontCollider.isTrigger = true;
            bottomCollider.isTrigger = true;
            frontCircle.isTrigger = true;
            backCircle.isTrigger = true;

            cameraController.PlayerDied();
            volumeController.PlayerDied();

            if (lastChunk){
                lastChunk.SetPlayerDead();
            }

            //Check if new high score, if so, save
            if (saveManager.state.highScore < score){
                saveManager.state.highScore = score;
                respawnMenu.NewHighScore(transform.position);
            }

            //Add last score to previous 10 list & delete last entry if over 10
            saveManager.state.lastScores.Insert(0, score);
            if (saveManager.state.lastScores.Count > 10){
                saveManager.state.lastScores.RemoveAt(saveManager.state.lastScores.Count - 1);
            }

            saveManager.Save();
        }
    }

    private void UpdateScore(int valuePopup){
        if (valuePopup > 0){
            //Have popup showing new value added for visual flair & clarity?
        }

        ScoreText.text = "Score: " + score.ToString("D4");
    }

    private void TouchStart(){
        touching = true;
        if (isGrounded){
            PlayAudio(jumpSFX, false, jumpSFXMod);
            ApplyJump(jumpForce);
        } else if (!isSlamming) {
            PlayAudio(slamSFX, false, slamSFXMod);
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(bottomCollider.bounds.center, bottomCollider.bounds.size, 0f, Vector2.down, groundedHeight, groundLayerMask);

        if (raycastHit.collider != null){
            if (raycastHit.transform.tag == "Switch" && rigi.velocity.y <= 0f){
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
                    PlayAudio(machineSFX, true, machineSFXMod);
                    PlayAudio(machinePointsSFX, true, machinePointsSFXMod);
                    Instantiate(machineBurstPrefab, transform.position, Quaternion.identity);
                } else {
                    PlayAudio(breakableSFX, true, breakableSFXMod);
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(frontCollider.bounds.center, frontCollider.bounds.size, 0f, Vector2.up, groundedHeight, groundLayerMask);

        if (raycastHit.collider != null && raycastHit.transform.gameObject.tag != "One-Way"){
            return true;
        }
        return false;
    }

    private bool CheckWall(){
        RaycastHit2D rightRaycastHit = Physics2D.BoxCast(frontCollider.bounds.center, RightCheckBoxCollider, 0f, Vector2.right, groundedHeight, groundLayerMask);

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
        if (!dead && started && !paused){
            if (context.ReadValue<UnityEngine.InputSystem.TouchPhase>() == UnityEngine.InputSystem.TouchPhase.Began){
                if (touching == false){
                    
                    //Make sure click was not on pause button. If so, ignore touch for player.
                    Touch touch = Input.GetTouch(0);
                    if (!pauseRegion.Contains(touch.position)){
                        TouchStart();
                    }
                }
            } else if (context.ReadValue<UnityEngine.InputSystem.TouchPhase>() == UnityEngine.InputSystem.TouchPhase.Ended){
                touching = false;
            }            
        }
    }

    //Called from Pause Menu, used to prevent extra player inputs while paused
    public void TogglePause(bool value){
        paused = value;
    }

    public void TouchBackup(InputAction.CallbackContext context){   //Keyboard & Gamepad alternatives to Touchscreen
        if (!dead && started && !paused){
            if (context.phase == InputActionPhase.Started){
                if (touching == false){
                    TouchStart();
                }
            } else if (context.phase == InputActionPhase.Canceled){
                touching = false;
            }
        }
    }

    public void PlayAudio(AudioClip audioClip, bool clipAtPoint, float volumeModifier){
        if (audioClip != null){
            if (clipAtPoint){
                currentAudioSource++;
                if (currentAudioSource >= rotatingAudioSource.Length){
                    currentAudioSource = 0;
                }

                rotatingAudioSource[currentAudioSource].clip = audioClip;
                rotatingAudioSource[currentAudioSource].volume = volume * volumeModifier;
                rotatingAudioSource[currentAudioSource].Play();
            } else {
                audioSource.volume = volume * volumeModifier;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }
    }

    public void PlayAudio(AudioClip[] audioClips, bool clipAtPoint, float volumeModifier){    //Overload function for choosing random sound effect from array to play
        if (audioClips.Length != 0){
            int rand = Random.Range(0, audioClips.Length-1);

            if (clipAtPoint){
                currentAudioSource++;
                if (currentAudioSource >= rotatingAudioSource.Length){
                    currentAudioSource = 0;
                }

                rotatingAudioSource[currentAudioSource].clip = audioClips[rand];
                rotatingAudioSource[currentAudioSource].volume = volume * volumeModifier;
                rotatingAudioSource[currentAudioSource].Play();
            } else {
                audioSource.volume = volume * volumeModifier;
                audioSource.clip = audioClips[rand];
                audioSource.Play();
            }
        }
    }

    public void FadeAudio(){
        audioSource.volume = audioSource.volume - (fadeSpeed * Time.deltaTime);
    }

    //Used to call chunk on death to prevent despawning
    public void SetLastChunk(ChunkExit chunk){
        lastChunk = chunk;
    }

    public bool GetDead(){
        return dead;
    }

    //Update points in LineRenderer to give TrailRenderer effect
    private void HandleTrail(){
        prevPositions.Insert(0, new Vector3(transform.position.x + trailXAdj, transform.position.y + trailYAdj, transform.position.z));
        prevPositions.RemoveAt(prevPositions.Count - 1);

        for (int i=0; i<trailLength; i++){
            lineRenderer.SetPosition(i, prevPositions[i]);
        }
    }

    //Shift points in lineRenderer when player is moved for seamless trail
    public void ShiftTrail(Vector3 shiftValue){
        for (int i=0; i<trailLength; i++){
            prevPositions[i] = new Vector3(prevPositions[i].x - shiftValue.x, prevPositions[i].y - shiftValue.y, prevPositions[i].z - shiftValue.z);
        }

        for (int i=0; i<trailLength; i++){
            lineRenderer.SetPosition(i, prevPositions[i]);
        }
    }

    public static Rect RectTransformToScreenSpace(RectTransform transform, Camera cam, bool cutDecimals = false)
    {
        var worldCorners = new Vector3[4];
        var screenCorners = new Vector3[4];

        transform.GetWorldCorners(worldCorners);

        for (int i = 0; i < 4; i++)
        {
            screenCorners[i] = cam.WorldToScreenPoint(worldCorners[i]);
            if (cutDecimals)
            {
                screenCorners[i].x = (int)screenCorners[i].x;
                screenCorners[i].y = (int)screenCorners[i].y;
            }
        }

        return new Rect(screenCorners[0].x,
                        screenCorners[0].y,
                        screenCorners[2].x - screenCorners[0].x,
                        screenCorners[2].y - screenCorners[0].y);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!dead){
            if (other.tag == "Worm" && !wormTrigger){
                //Prevent multiple trigger fire bug
                wormTrigger = true;

                score += wormValue;
                PlayAudio(collectSFX, true, wormSFXMod);
                UpdateScore(wormValue);
                Instantiate(wormBurstPrefab, other.transform.position, wormBurstPrefab.transform.rotation);
                other.gameObject.SetActive(false);
                saveManager.state.wormCount += 1;
            } else if (other.tag == "Hurt"){
                Die();
            } else if (other.tag == "Spring" && !springTrigger){
                springTrigger = true;
                Vector2 Forces = other.GetComponent<Spring>().LaunchSpring();

                isSlamming = false;
                anim.SetBool("Dashing", false);
                PlayAudio(springSFX, true, springSFXMod);

                ApplyJump(Forces.y);

                if (Forces.x != 0){
                    moveSpring = Forces.x;
                }

                //Should add Anti-Slam prevention after very start of spring launch? Small counter to prevent launch momemtum accidently being stopped?
            } else if (other.tag == "End"){
                saveManager.Save(); //Ensures tutorial worms saved
                GameObject TransitionObj = Instantiate(transitionPrefab, new Vector3(transform.position.x + 10, transform.position.y, transform.position.z), Quaternion.identity);
                TransitionObj.GetComponent<Transition>().SetValues("Title", transform.position.x - 3f, -40);
            } else if (other.tag == "Conveyor"){
                conveyorSpeed = other.GetComponent<Conveyor>().xSpeed;
                onConveyor = true;
            }
        }
    }

    //Check for rare ghost collision between two equal height boxColliders on mole feet preventing forward movement
    //Also checks for rare edge stuck bug where all movement halts, forcing player forward very slightly for a different collision check
    private void OnCollisionEnter2D(Collision2D other) {
        if ((isGrounded && !isSlamming && !pushingWall && started && rigi.velocity.x == 0) || 
            (!isGrounded && !isSlamming && started && rigi.velocity.x == 0 && rigi.velocity.y == 0) || 
            (!isGrounded && !isSlamming && started && transform.position.x == ghostPrevX && transform.position.y == ghostPrevY)){
            Debug.Log("Rare edge stuck! Moving slightly...");
            transform.position = new Vector3(transform.position.x + ghostCollisionAdd, transform.position.y + ghostCollisionAdd, transform.position.z);
            ghostPrevX = transform.position.x;
            ghostPrevY = transform.position.y;
        } else if (!isGrounded && !isSlamming && started){
            ghostPrevX = transform.position.x;
            ghostPrevY = transform.position.y;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.tag == "Conveyor"){
            onConveyor = false;
        }
    }
}