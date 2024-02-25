using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpike : MonoBehaviour
{
    public float xSpeed;

    private float yLim = -50f;

    void Update()
    {
        transform.position = new Vector3(transform.position.x + (xSpeed * Time.deltaTime), transform.position.y, transform.position.z);

        if (transform.position.y < yLim){
            gameObject.SetActive(false);
        }
    }
}
