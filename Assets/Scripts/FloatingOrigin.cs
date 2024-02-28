using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingOrigin : MonoBehaviour
{
    public float threshold;
    public TerrainController terrainController;

    private void LateUpdate() {
        Vector3 cameraPosition = gameObject.transform.position;
        cameraPosition.y = 0f;
        cameraPosition.z = 0f;

        if (cameraPosition.magnitude > threshold){
            for (int z=0; z < SceneManager.sceneCount; z++){
                foreach(GameObject g in SceneManager.GetSceneAt(z).GetRootGameObjects()){
                    g.transform.position -= cameraPosition;
                }
            }

            Vector3 originDelta = Vector3.zero - cameraPosition;
            terrainController.AdjustLastGenX();
            Debug.Log("Recentering, origin delta = " + originDelta);
        }
    }
}
