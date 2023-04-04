using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstRandomization : MonoBehaviour
{
    public GameObject[] BurstObjects;
    public Vector2 xForceLimits;
    public Vector2 yForceLimits;

    void Start()
    {
        for (int i=0; i< BurstObjects.Length; i++){
            float xForce = Random.Range(xForceLimits.x, xForceLimits.y);
            float yForce = Random.Range(yForceLimits.x, yForceLimits.y);
            BurstObjects[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce));
        }
    }

    void Update()
    {
        if (BurstObjects[0].transform.position.y < -60){
            Destroy(gameObject);
        }
    }
}
