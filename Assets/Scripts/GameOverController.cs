using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retry(){
        SceneManager.LoadScene("SampleScene");

    }

    public void ReturnToTitle(){
        SceneManager.LoadScene("Title");
    }
}
