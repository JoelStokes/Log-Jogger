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
    public int hardTotal;
    private int currentDifficulty = 0;
    private int difficultyCount = 0;

    //Prevent repeats until all from group are used
    List<int> usedChunks = new List<int>();

    private float lastGenX;

    void OnEnable(){
        ChunkExit.OnChunkExited += PickAndSpawnChunk;
    }

    private void OnDisable(){
        ChunkExit.OnChunkExited -= PickAndSpawnChunk;
    }

    private void Start(){
        lastGenX = transform.position.x;
    }

    private void IncreaseDifficulty(){
        difficultyCount = 0;
        if (currentDifficulty < 2){
            currentDifficulty++;
        }
        usedChunks.Clear();
    }

    void PickAndSpawnChunk(){
        int random = -1;    //Set to -1 to ensure random value gets properly set
        int counter = 0;

        if ((currentDifficulty == 0 && difficultyCount >= easyTotal) || 
            (currentDifficulty == 1 && difficultyCount >= mediumTotal) || 
            (currentDifficulty == 2 && difficultyCount >= hardTotal)){
            IncreaseDifficulty();
        } else {
            difficultyCount++;
        }

        do {
            counter++;
            if (counter > 100){
                Debug.Log("ERROR! Infinite loop, no free chunk found!");
                Debug.Log(usedChunks);
                break;
            }

            switch(currentDifficulty){
                case 0:
                    random = Random.Range(0, EasyPrefabs.Length);
                    break;
                case 1:
                    random = Random.Range(0, MediumPrefabs.Length);
                    break;
                case 2:
                    random = Random.Range(0, HardPrefabs.Length);
                    break;
                default:
                    Debug.Log("ERROR! invalid difficulty number in PickAndSpawnChunk!");
                    random = 0;
                    usedChunks.Clear();
                    break;
            }
        } while (usedChunks.Contains(random));

        //Creation to be done in Coroutine?
        InstantiateChunk(random);
    }

    void InstantiateChunk(int value){
        GameObject newObj;

        switch(currentDifficulty){
            case 0:
                newObj = GameObject.Instantiate(EasyPrefabs[value], new Vector3(lastGenX, transform.position.y, 0.5f), Quaternion.identity);
                break;
            case 1:
                newObj = GameObject.Instantiate(MediumPrefabs[value], new Vector3(lastGenX, transform.position.y, 0.5f), Quaternion.identity);
                break;
            case 2:
                newObj = GameObject.Instantiate(HardPrefabs[value], new Vector3(lastGenX, transform.position.y, 0.5f), Quaternion.identity);
                break;
            default:
                Debug.Log("ERROR! invalid difficulty number in InstantiateChunk!");
                newObj = GameObject.Instantiate(EasyPrefabs[0], new Vector3(lastGenX, transform.position.y, 0.5f), Quaternion.identity);
                break;
        }
        
        usedChunks.Add(value);
        lastGenX = newObj.transform.Find("Connector").position.x;
    }

    //1. Generate new chunk when close enough
    //2. Disable previous chunk 10 seconds after passing / once offscreen?
    //3. Creation needs to be done with some sort of coroutine setup?

    //4:00 Level Layout OnEnable subscribes to OnChunkExited event
}
