using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    void Start()
    {
        
    }

    public void Play(){
        SceneManager.LoadScene("SampleScene");
    }

    public void Tutorial(){
        SceneManager.LoadScene("Tutorial");
    }

    public void HighScore(){
        SceneManager.LoadScene("HighScore");
    }

    public void Settings(){
        //Replace buttons with Settings options
    }

    public void Quit(){
        Application.Quit();
    }
}
