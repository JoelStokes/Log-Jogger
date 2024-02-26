using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkExit : MonoBehaviour
{
    public float delay = 8f;

    public delegate void ExitAction();
    public static event ExitAction OnChunkExited;

    private bool exited = false;
    private bool playerDead = false;
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player"){
            if (!exited){
                exited = true;
                other.gameObject.GetComponent<PlayerController>().SetLastChunk(this);

                OnChunkExited();
                StartCoroutine(WaitAndDeactivate());
            }
        }
    }

    public void SetPlayerDead(){
        playerDead = true;
    }

    IEnumerator WaitAndDeactivate(){
        yield return new WaitForSeconds(delay);

        //Make sure player has not died since coroutine started to prevent despawning blocks near the dead player
        if (!playerDead){
            transform.parent.gameObject.SetActive(false);
        }
    }
}
