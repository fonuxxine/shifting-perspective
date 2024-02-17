using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float motionSpeed = 0.5f; // Speed of the motion loop

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float loopPosition = 0.0f;
    private Transform parentObject; // Parent object to move

    private Transform placeholderObject; // The object to move towards

    void Start()
    {
        placeholderObject = transform;

        // Retrieve the parent object to move
        parentObject = transform.parent;

        // Store initial position and rotation relative to the parent
        initialPosition = parentObject.InverseTransformPoint(placeholderObject.position);
        initialRotation = Quaternion.Inverse(parentObject.rotation) * placeholderObject.rotation;
    }

    void Update()
    {
        // Calculate loop position
        loopPosition += Time.deltaTime * motionSpeed;

        // Calculate target position and rotation relative to the parent
        Vector3 targetPosition = parentObject.TransformPoint(Vector3.Lerp(initialPosition, -initialPosition, Mathf.PingPong(loopPosition, 1.0f)));
        Quaternion targetRotation = parentObject.rotation * Quaternion.Lerp(initialRotation, Quaternion.Inverse(initialRotation), Mathf.PingPong(loopPosition, 1.0f));

        // Move towards target position
        parentObject.position = Vector3.Lerp(parentObject.position, targetPosition, Time.deltaTime * motionSpeed);
        // Rotate towards target rotation
        parentObject.rotation = Quaternion.Lerp(parentObject.rotation, targetRotation, Time.deltaTime * motionSpeed);
    }
}
