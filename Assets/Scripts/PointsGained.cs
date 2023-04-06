using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsGained : MonoBehaviour
{
    public TextMeshPro Text;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimFinished(){
        Destroy(gameObject);
        //Seems like a wasteful way to do it, figure out some sort of object pooling method?
    }
}