using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float smoothSpeed = 5f; // Adjust this value to control the smoothness of the camera follow

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position for the camera
            var position = transform.position;
            var position1 = target.position;
            
            Vector3 desiredPosition = new Vector3(position1.x, position1.y, position.z);

            // Smoothly move the camera towards the desired position
            position = Vector3.Lerp(position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = position;
        }
    }
}