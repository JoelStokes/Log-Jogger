using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    public enum BGLayer{
        Bottom, Mountains, Clouds, FarClouds, Sky
    };

    public BGLayer bgLayer;

    private float bottomMult = .55f;
    private float cloudsMult = .65f;
    private float mountainsMult = .75f;
    private float farCloudsMult = .85f;
    private float sky = 1;

    private float startY;
    private float startX;

    private GameObject CameraObj;

    void Start()
    {
        startX = transform.position.x;
        startY = transform.position.y;

        CameraObj = Camera.main.gameObject;
    }

    void Update()
    {
        float scale;
        switch (bgLayer){
            case BGLayer.Bottom:
                scale = bottomMult;
                break;
            case BGLayer.Mountains:
                scale = mountainsMult;
                break;
            case BGLayer.Clouds:
                scale = cloudsMult;
                break;
            case BGLayer.FarClouds:
                scale = farCloudsMult;
                break;
            default:
                scale = sky;
                break;
        }
        transform.position = new Vector3((CameraObj.transform.position.x + startX) * scale, (CameraObj.transform.position.y + startY) * scale, transform.position.z);
    }
}