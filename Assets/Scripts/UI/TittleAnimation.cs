using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float scaleAmount = 0.1f;
    public float maxRotationAngle = 15.0f;

    void Update()
    {
        // Move the title text slightly
        Vector3 newPosition = transform.position;
        newPosition.x += Mathf.Sin(Time.time * moveSpeed) * 0.1f; // Adjust the multiplier for the desired movement range
        transform.position = newPosition;

        // Scale the title text slightly
        float scale = 1.0f + Mathf.Sin(Time.time * moveSpeed) * scaleAmount;
        transform.localScale = new Vector3(scale, scale, 1.0f);

        // Rotate the title text slightly (limited to maxRotationAngle)
        float rotation = Mathf.Sin(Time.time * moveSpeed) * maxRotationAngle;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation);
    }
}