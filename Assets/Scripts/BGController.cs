using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    public enum BGLayer{
        Bottom, Mountains, Clouds, FarClouds, Sky
    };

    public BGLayer bgLayer;
    public float bgMoveX;

    private float bottomMult = .55f;
    private float cloudsMult = .65f;
    private float mountainsMult = .75f;
    private float farCloudsMult = .85f;
    private float sky = 1;

    private float cloudSpeed = -.3f;
    private float farCloudSpeed = -.08f;

    private float startY;
    private float startX;

    private float loopX = -15f;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private bool stopScroll = false;

    void Start()
    {
        startX = transform.position.x;
        startY = transform.position.y;

        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        //Move element when offscreen farther into the level
        if (transform.position.x - cameraTransform.position.x < loopX && !stopScroll){
            transform.position = new Vector3(cameraTransform.position.x + bgMoveX, transform.position.y, transform.position.z);
        }

        transform.position += deltaMovement * GetLayerValue();
        lastCameraPosition = cameraTransform.position;

        if (bgLayer == BGLayer.Clouds){
            transform.position = new Vector3(transform.position.x + (cloudSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        } else if (bgLayer == BGLayer.FarClouds){
            transform.position = new Vector3(transform.position.x + (farCloudSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
    }

    private float GetLayerValue(){
        switch (bgLayer){
            case BGLayer.Bottom:
                return bottomMult;
            case BGLayer.Mountains:
                return mountainsMult;
            case BGLayer.Clouds:
                return cloudsMult;
            case BGLayer.FarClouds:
                return farCloudsMult;
            default:
                return sky;
        }
    }

    //Stop BG camera scroll, used during Floating Origin recentering
    public void ToggleScroll(){
        stopScroll = !stopScroll;

        //Camera value will dramatically change during floating origin recenter, must be reset on toggle
        lastCameraPosition = cameraTransform.position;
    }
}