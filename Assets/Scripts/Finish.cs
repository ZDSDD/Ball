using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        //todo: Implement logic to end the level. UI stuff
        if(other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            Debug.Log("BRAVO, YOU FINISHED!");
            playerController.Rb.velocity = Vector2.zero;
            playerController.Rb.gravityScale = 0f;
            StartCoroutine(MovePlayerToFinish(playerController));
            playerController.LevelComplete = true;
            playerController.onLevelComplete.Invoke();
        }
    }
    /**
     * Move player slowly to the finish center
     */
    private IEnumerator MovePlayerToFinish(PlayerController playerController)
    {
        float duration = 1.5f; // Adjust the duration as needed
        Vector3 startPosition = playerController.transform.position;
        Vector3 targetPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            playerController.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the player is exactly at the target position
        playerController.transform.position = targetPosition;
        
    }
}
