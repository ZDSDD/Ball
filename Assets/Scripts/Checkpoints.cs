using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public PlayerController playerController;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(this.tag == "FinishPoint")
            {
                Debug.Log("Congratulations!");
            }
            playerController.UpdateCheckpoint(new Vector2(this.transform.position.x, this.transform.position.y));
            playerController.Reset();
        }
    }
}
