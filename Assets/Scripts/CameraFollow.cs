using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 _cameraOffset = new Vector3(0, 1, -3);
    [Range(0.01f, 1.0f)] public float smoothFactor = 0.125f;

    void Update()
    {
        Vector3 desiredPosition = playerTransform.position + _cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothFactor);
        transform.position = smoothedPosition;
        
        transform.LookAt(playerTransform);
    }
}
