using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used for Hard Chunks that are reused in the level to ensure all previous objects are back
public class ChunkReloader : MonoBehaviour
{
    public GameObject[] Worms;
    public GameObject[] Rocks;
    public GameObject[] Machines;

    void OnEnable()
    {
        for (int i=0; i<Worms.Length; i++){
            Worms[i].SetActive(true);
        }

        for (int i=0; i<Rocks.Length; i++){
            Rocks[i].SetActive(true);
        }

        for (int i=0; i<Machines.Length; i++){
            Machines[i].SetActive(true);
        }
    }
}
