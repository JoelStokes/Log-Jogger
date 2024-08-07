using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingOrigin : MonoBehaviour
{
    public float threshold;
    public TerrainController terrainController;
    public BGController[] bgControllers;

    private void LateUpdate() {
        Vector3 cameraPosition = gameObject.transform.position;
        cameraPosition.y = 0f;
        cameraPosition.z = 0f;

        if (cameraPosition.magnitude > threshold){
            ToggleBGs();

            for (int z=0; z < SceneManager.sceneCount; z++){
                foreach(GameObject g in SceneManager.GetSceneAt(z).GetRootGameObjects()){
                    if (g.layer== LayerMask.NameToLayer("Blur")){
                        foreach(Transform child in g.transform){
                            Debug.Log("Applying move to children");
                            child.transform.position -= cameraPosition;
                        }
                    } else if (g.tag == "Player"){
                        g.transform.position -= cameraPosition;
                        g.GetComponent<PlayerController>().ShiftTrail(cameraPosition);
                    } else {
                        g.transform.position -= cameraPosition;
                    }
                }
            }

            Vector3 originDelta = Vector3.zero - cameraPosition;
            terrainController.AdjustLastGenX();
            //Debug.Log("Recentering, origin delta = " + originDelta);
            
            ToggleBGs();
        }
    }

    private void ToggleBGs(){
        foreach(BGController i in bgControllers){
            i.ToggleScroll();
        }
    }
}
