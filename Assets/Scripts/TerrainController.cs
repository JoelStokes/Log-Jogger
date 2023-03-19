using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    private List<GameObject> TerrainChunks = new List<GameObject>();

    private float scrollSpeed = 5;
    private float destroyX = -20;
    private float generateX = 20;
    private bool scrolling = true;

    void Start()
    {
        TerrainChunks.Add(transform.GetChild(0).gameObject);
    }

    void Update()
    {
        if (scrolling){
            float speed = scrollSpeed * Time.deltaTime;

            /*for (int i=0; i<TerrainChunks.Count; i++){
                TerrainChunks[i].transform.position = new Vector3(TerrainChunks[i].transform.position.x - speed, 
                    TerrainChunks[i].transform.position.y, 
                    TerrainChunks[i].transform.position.z);

                if (transform.position.x < destroyX){
                    TerrainChunks.RemoveAt(i);
                }
            }*/
        }
    }

    public void PauseScroll(){  //Player Slam, against wall, or death
        scrolling = false;
    }

    public void ResumeScroll(){
        scrolling = true;
    }
}
