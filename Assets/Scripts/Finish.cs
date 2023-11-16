using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //todo: Implement logic to end the level. UI stuff
        if(other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            Debug.Log("BRAVO, YOU FINISHED!");
            playerController.Reset();
        }
    }
}
