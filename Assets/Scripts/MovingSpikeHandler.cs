using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Have an array of MovingSpike objects that use object pooling
public class MovingSpikeHandler : MonoBehaviour
{
    public GameObject SpikePrefab;
    public float spawnRate;
    public GameObject[] ExistingSpikes;

    private float spawnTimer = 0;
    private List<GameObject> Spikes = new List<GameObject>();

    //Create & run first spike
    void Start()
    {
        //Take any pre-placed spikes and add them to spike pool
        for (int i=0; i<ExistingSpikes.Length; i++){
            Spikes.Add(ExistingSpikes[i]);
        }
        
        CreateSpike();
        PrepareSpike(Spikes.Count-1);
    }

    void Update()
    {
        if (spawnTimer >= spawnRate){
            PrepareSpike(GetNextSpike());
            spawnTimer = 0;
        } else {
            spawnTimer += Time.deltaTime;
        }
    }

    //Get next available spike in pool. If empty, instantiate new spike.
    private int GetNextSpike(){
        for (int i=0; i < Spikes.Count; i++){
            if (!Spikes[i].activeSelf){
                return i;
            }
        }

        CreateSpike();
        return Spikes.Count-1;
    }

    private void CreateSpike(){
        GameObject NewSpike = GameObject.Instantiate(SpikePrefab, transform.position, Quaternion.identity);
        Spikes.Add(NewSpike);
    }

    //Prepare spike for use
    public void PrepareSpike(int spikeNumber){
        Spikes[spikeNumber].transform.position = transform.position;
        Spikes[spikeNumber].transform.eulerAngles = Vector3.zero;
        Spikes[spikeNumber].SetActive(true);
    }
}
