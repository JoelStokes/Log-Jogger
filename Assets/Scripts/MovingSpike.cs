using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpike : MonoBehaviour
{
    public float xSpeed;
    public MovingSpikeHandler movingSpikeHandler;

    private float yLim = -50f;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + (xSpeed * Time.deltaTime), transform.position.y, transform.position.z);

        if (transform.position.y < yLim){
            gameObject.setActive(false);
            movingSpikeHandler.
        }
    }
}
