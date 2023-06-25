using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    public GameObject[] EasyPrefabs;
    public GameObject[] MediumPrefabs;
    public GameObject[] HardPrefabs;

    public int easyTotal;
    public int mediumTotal;

    private List<GameObject> TerrainChunks = new List<GameObject>();

    private float lastGenX;

    //1. Spawn in prefabs from scene load & place them in order
    //2. Deactivate Chunk when offscreen for memory
    //3. Rework collectibles to be regenerated for level chunk pooling
            //3a. Because of set amount of Easy & Medium Prefabs, only Hard have to be pooled?

    void Start()
    {
        lastGenX = transform.position.x;
        
        GenerateChunks(EasyPrefabs, easyTotal);
        //GenerateChunks(MediumPrefabs, mediumTotal);
        //GenerateChunks(HardPrefabs, HardPrefabs.Length);
    }

    private void GenerateChunks(GameObject[] LevelPrefabs, int total){
        List<int> usedPrefabs = new List<int>();
        int random;

        for (int i=0; i<total; i++){
            if (usedPrefabs.Count != LevelPrefabs.Length){  //Prevent infinite loop
                do {
                    random = Random.Range(0, LevelPrefabs.Length);
                } while (usedPrefabs.Contains(random));

                GameObject newObj = GameObject.Instantiate(LevelPrefabs[random], new Vector3(lastGenX, transform.position.y, 0.5f), Quaternion.identity);          
                
                lastGenX = newObj.transform.Find("Connector").position.x;
                usedPrefabs.Add(random);
            }
        }
    }
}
