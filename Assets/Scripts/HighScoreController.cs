using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreController : MonoBehaviour
{
    public GameObject transitionPrefab;

    public TextMeshPro[] personalBestUI;

    private float transitionX = 12;
    private float transitionXEnd = -9;

    void Start(){
        SaveManager saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();

        for (int i=0; i<personalBestUI.Length; i++){
            if (saveManager.state.lastScores[i] > 0){   //List pre-populated with 0s on new save setup
                personalBestUI[i].text = saveManager.state.lastScores[i].ToString();
            } else {
                personalBestUI[i].text = "-";
            }
        }

        //highScoreUI.text = saveManager.state.highScore.ToString("0000");
    }

    public void MainMenu(){
        CreateTransition("Title");
    }

    private void CreateTransition (string scene){
        GameObject TransitionObj = Instantiate(transitionPrefab, new Vector3(transitionX, transform.position.y, 0), Quaternion.identity);
        TransitionObj.GetComponent<Transition>().SetValues(scene, transitionXEnd, -20, .4f);        
    }
}
