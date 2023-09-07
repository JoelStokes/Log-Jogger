using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashscreenController : MonoBehaviour
{
    private float timer = 0;
    private Animator textAnim;

    void Start()
    {
        textAnim = GetComponent<Animator>();
    }

    public void LoadTitle(){
        SceneManager.LoadScene("Title");
    }

    public void StartTextAnim(){
        textAnim.SetTrigger("Start");
    }
}
