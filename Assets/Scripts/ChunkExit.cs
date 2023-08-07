using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkExit : MonoBehaviour
{
    public float delay = 5f;

    public delegate void ExitAction();
    public static event ExitAction OnChunkExited;

    private bool exited = false;
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player"){
            if (!exited){
                exited = true;
                OnChunkExited();
                StartCoroutine(WaitAndDeactivate());
            }
        }
    }

    IEnumerator WaitAndDeactivate(){
        yield return new WaitForSeconds(delay);

        transform.root.gameObject.SetActive(false);
    }
}
