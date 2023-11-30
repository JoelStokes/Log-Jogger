using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 startPos;
    public float startSize;

    public SpriteRenderer fadeSprite;
    public float fadeValue;

    public float lerpSpeed;
    public float deadLerpSpeed;
    private float lerpTime = 0;

    private bool isDead;
    private Vector3 playPos;
    private float playSize;
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;

        playPos = transform.localPosition;
        playSize = cam.orthographicSize;

        transform.localPosition = startPos;
        cam.orthographicSize = startSize;
    }

    void Update()
    {
        //Add check to see if player is below threshold to follow for off-screen death?

        if (!isDead){
            transform.localPosition = new Vector3(HandleLerp(startPos.x, playPos.x), HandleLerp(startPos.y, playPos.y), transform.localPosition.z);
            cam.orthographicSize =  HandleLerp(startSize, playSize);
        } else {
            transform.localPosition = new Vector3(HandleLerp(playPos.x, startPos.x), HandleLerp(playPos.y, startPos.y), transform.localPosition.z);
            cam.orthographicSize =  HandleLerp(playSize, startSize);
            fadeSprite.color = new Vector4(0,0,0, HandleLerp(0, fadeValue));
        }

        lerpTime += lerpSpeed * Time.deltaTime;
    }

    private float HandleLerp (float startValue, float endValue){
        return Mathf.Lerp(startValue, endValue, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, lerpTime)));
    }

    public void PlayerDied(){
        isDead = true;
        lerpTime = 0;
    }
}
