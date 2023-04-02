using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimSpeed : MonoBehaviour
{
    public float min;
    public float max;

    void Start()
    {
        float random = Random.Range(min, max);
        GetComponent<Animator>().speed = random;
    }
}
