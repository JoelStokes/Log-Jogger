using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreController : MonoBehaviour
{
    public GameObject transitionPrefab;

    public TextMeshProUGUI highScoreUI;

    private float transitionX = 12;
    private float transitionXEnd = -9;

    void Start(){
        SaveManager saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();

        highScoreUI.text = saveManager.state.highScore.ToString("0000");
    }

    public void MainMenu(){
        CreateTransition("Title");
    }

    private void CreateTransition (string scene){
        GameObject TransitionObj = Instantiate(transitionPrefab, new Vector3(transitionX, transform.position.y, 0), Quaternion.identity);
        TransitionObj.GetComponent<Transition>().SetValues(scene, transitionXEnd, -20, .4f);        
    }
}
