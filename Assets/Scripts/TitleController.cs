using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Play(){
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit(){
        Application.Quit();
    }
}
