using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leverActivate : MonoBehaviour
{
    public Transform gateTransform;
    public float gateMoveDistance = 2f; // Adjust as needed
    public float activationRange = 3f; // Adjust as needed
    public float leverRotationAngle = -90f; // Adjust as needed
    public float leverRotateSpeed = 2f; // Adjust as needed
    public float gateOpenSpeed = 2f; // Adjust as needed
    public Color detectionColor = Color.yellow; // Color when in detection range
    public Color originalColor = Color.white; // Color when not in detection range

    private bool isLeverActivated = false;
    private Vector3 initialGatePosition;
    private Quaternion originalLeverRotation;
    private Material leverMaterial;

    private void Start()
    {
        // Store the initial position of the gate and rotation of the lever
        initialGatePosition = gateTransform.position;
        originalLeverRotation = transform.rotation;

        // Get the lever's material (assuming it's a single material)
        Renderer leverRenderer = GetComponent<Renderer>();
        if (leverRenderer != null)
        {
            leverMaterial = leverRenderer.material;
        }

        // Set the initial color of the lever
        SetLeverColor(originalColor);
    }

    private void Update()
    {
        // Check if the player is within the activation range
        Collider[] colliders = Physics.OverlapSphere(transform.position, activationRange);
        bool playerInRange = false;
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerInRange = true;
                break;
            }
        }

        // Change the lever color based on player proximity
        SetLeverColor(playerInRange ? detectionColor : originalColor);

        // Check for player input (e.g., pressing a key or tapping the screen)
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && !isLeverActivated)
        {
            ActivateLever();
        }
    }

    private void ActivateLever()
    {
        StartCoroutine(RotateLeverSmoothly(leverRotationAngle));
        // Gradually open the gate
        Vector3 targetGatePosition = initialGatePosition + Vector3.up * gateMoveDistance;
        StartCoroutine(OpenGateSmoothly(targetGatePosition));

        // Lever is now activated
        isLeverActivated = true;
    }

    private IEnumerator RotateLeverSmoothly(float targetAngle)
    {
        float elapsedTime = 0f;
        Quaternion targetRotation = originalLeverRotation * Quaternion.Euler(targetAngle, 0f, 0f);

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(originalLeverRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * leverRotateSpeed;
            yield return null;
        }

        // Ensure the lever reaches the exact target rotation
        transform.rotation = targetRotation;
    }

    private IEnumerator OpenGateSmoothly(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = gateTransform.position;

        while (elapsedTime < 1f)
        {
            gateTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * gateOpenSpeed;
            yield return null;
        }

        // Ensure the gate reaches the exact target position
        gateTransform.position = targetPosition;
    }

    private void SetLeverColor(Color color)
    {
        if (leverMaterial != null)
        {
            leverMaterial.color = color;
        }
    }

    // Visualize the activation range in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = detectionColor;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }
}