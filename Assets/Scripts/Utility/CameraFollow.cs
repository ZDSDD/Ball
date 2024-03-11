using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraFollow : MonoBehaviour
{
    public PlayerController target; // Reference to the player's transform
    public float smoothSpeed = 5f; // Adjust this value to control the smoothness of the camera follow
    private bool targetSet = false;
    private float _initialSize;
    public Camera camera;

    private void Start()
    {
        camera = Camera.main;
        Assert.IsNotNull(camera);
        target = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (target)
        {
            targetSet = true;
            target.LaunchPowerChanged += OnPowerChange;
            target.LaunchEnd += OnLaunched;
        }

        _initialSize = camera.orthographicSize;
    }

    private void OnLaunched()
    {
        StartCoroutine(MoveCameraToInitialPosition());
    }

    private IEnumerator MoveCameraToInitialPosition()
    {
        float duration = 1.5f; // Adjust the duration as needed
        float startPosition = camera.orthographicSize;
        float targetPosition = _initialSize;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            camera.orthographicSize = Mathf.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the player is exactly at the target position
        camera.orthographicSize = targetPosition;
    }

    public void OnPowerChange(float power)
    {
        if (power == 0)
        {
            power = _initialSize;
        }
        else
        {
            power = (float)Math.Log(power) + _initialSize;
        }
        camera.orthographicSize = power;
    }

    void LateUpdate()
    {
        if (!targetSet) return;

        // Calculate the desired position for the camera
        var position = transform.position;
        var position1 = target.transform.position;

        Vector3 desiredPosition = new Vector3(position1.x, position1.y, position.z);

        // Smoothly move the camera towards the desired position
        position = Vector3.Lerp(position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = position;
    }
}