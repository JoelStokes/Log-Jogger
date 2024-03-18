using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPItHandler : MonoBehaviour
{
    private float behindX = -25f;
    private float resetX = 58.5f;

    private GameObject CameraObj;

    void Start()
    {
        CameraObj = Camera.main.gameObject;
    }

    void LateUpdate()
    {
        if (transform.position.x <= CameraObj.transform.position.x + behindX){
            transform.position = new Vector3(transform.position.x + resetX, transform.position.y, transform.position.z);
        }
    }
}
