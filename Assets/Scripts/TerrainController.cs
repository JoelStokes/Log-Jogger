using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    public GameObject[] EasyChunks;
    public GameObject[] MediumChunks;
    public GameObject[] HardChunks;

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
        difficultyCount = 1;
        if (currentDifficulty < 2){
            currentDifficulty++;
        }
        usedChunks.Clear();
    }

    void PickAndSpawnChunk(){
        int random = -1;    //Set to -1 to ensure random value gets properly set
        int counter = 0;

        Debug.Log("Before Difficulty: " + currentDifficulty + ", difficultyCount: " + difficultyCount + "| Used Chunks: " + usedChunks.Count);
        if ((currentDifficulty == 0 && difficultyCount >= easyTotal) || 
            (currentDifficulty == 1 && difficultyCount >= mediumTotal) ||
            (currentDifficulty == 2 && difficultyCount >= hardTotal)){
            IncreaseDifficulty();
        } else {
            difficultyCount++;
        }

        Debug.Log("After Difficulty: " + currentDifficulty + ", difficultyCount: " + difficultyCount + "| Used Chunks: " + usedChunks.Count);

        do {
            counter++;
            if (counter > 100){
                Debug.Log("ERROR! Infinite loop, no free chunk found!");
                Debug.Log(usedChunks);
                break;
            }

            switch(currentDifficulty){
                case 0:
                    random = Random.Range(0, EasyChunks.Length);
                    break;
                case 1:
                    random = Random.Range(0, MediumChunks.Length);
                    break;
                case 2:
                    random = Random.Range(0, HardChunks.Length);
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
        switch(currentDifficulty){
            case 0:
                EasyChunks[value].transform.position = new Vector3(lastGenX, transform.position.y, 0.5f);
                EasyChunks[value].SetActive(true);
                lastGenX = EasyChunks[value].transform.Find("Connector").position.x;
                break;
            case 1:
                MediumChunks[value].transform.position = new Vector3(lastGenX, transform.position.y, 0.5f);
                MediumChunks[value].SetActive(true);
                lastGenX = MediumChunks[value].transform.Find("Connector").position.x;
                break;
            case 2:
                HardChunks[value].transform.position = new Vector3(lastGenX, transform.position.y, 0.5f);
                HardChunks[value].SetActive(true);
                lastGenX = HardChunks[value].transform.Find("Connector").position.x;
                if (usedChunks.Count >= hardTotal){     //Ensure recent chunk not called again too early.
                    usedChunks.RemoveAt(0);
                }
                break;
            default:
                Debug.Log("ERROR! invalid difficulty number in InstantiateChunk!");
                break;
        }
        
        usedChunks.Add(value);
    }
}
