using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    public float yScale;

    private GameObject CameraObj;

    void Start()
    {
        CameraObj = Camera.main.gameObject;
    }

    void Update()
    {
        transform.position = new Vector3(CameraObj.transform.position.x, CameraObj.transform.position.y * yScale, transform.position.z);
    }
}
