using UnityEngine;

public class CheckpointHit : MonoBehaviour
{
    // Active checkpoint material
    public Material greenMaterial;

    // Prior checkpoint material
    public Material yellowMaterial;

    // Reference to the RespawnPoint GameObject
    public GameObject respawnPoint;

    // Radius to detect the player
    public float detectionRadius = 2f;

    // Variable to track the current checkpoint number
    private int currentCheckpoint = 0;

    // Array to store previously hit checkpoints
    private GameObject[] previousCheckpoints;

    void Start()
    {
        previousCheckpoints = new GameObject[0];
    }

    void Update()
    {
        // Check if any objects with name "Checkpoint<int>" are within range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.name.StartsWith("Checkpoint"))
            {
                int checkpointNumber = GetCheckpointNumber(hitCollider.gameObject.name);
                Renderer renderer = hitCollider.gameObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    if (checkpointNumber > currentCheckpoint)
                    {
                        // Change material to green
                        foreach (var checkpoint in previousCheckpoints)
                        {
                            checkpoint.GetComponent<Renderer>().material = yellowMaterial;
                        }
                        renderer.material = greenMaterial;
                        currentCheckpoint = checkpointNumber;
                        // Update previously hit checkpoints array
                        UpdatePreviousCheckpoints(hitCollider.gameObject);

                        // Move the RespawnPoint to the location of the child "RespawnPlaceholder" under the checkpoint
                        MoveRespawnPoint(hitCollider.gameObject.transform);
                    }
                    else if (checkpointNumber < currentCheckpoint)
                    {
                        // Change material to yellow
                        renderer.material = yellowMaterial;
                        // Update previously hit checkpoints array
                        UpdatePreviousCheckpoints(hitCollider.gameObject);
                    }
                }
            }
        }
    }

    // Visualize the detection radius in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // Helper method to extract the checkpoint number from the GameObject name
    private int GetCheckpointNumber(string checkpointName)
    {
        string numberString = checkpointName.Substring("Checkpoint".Length);
        return int.Parse(numberString);
    }

    // Update previously hit checkpoints array
    private void UpdatePreviousCheckpoints(GameObject newCheckpoint)
    {
        GameObject[] newPreviousCheckpoints = new GameObject[previousCheckpoints.Length + 1];
        for (int i = 0; i < previousCheckpoints.Length; i++)
        {
            newPreviousCheckpoints[i] = previousCheckpoints[i];
        }
        newPreviousCheckpoints[newPreviousCheckpoints.Length - 1] = newCheckpoint;
        previousCheckpoints = newPreviousCheckpoints;
    }

    // Move the RespawnPoint to the position of the child "RespawnPlaceholder" under the given checkpoint
    private void MoveRespawnPoint(Transform checkpointTransform)
    {
        if (respawnPoint != null)
        {
            Transform respawnPlaceholder = checkpointTransform.Find("RespawnPlaceholder");
            if (respawnPlaceholder != null)
            {
                respawnPoint.transform.position = respawnPlaceholder.position;
            }
            else
            {
                Debug.LogWarning("No RespawnPlaceholder found under the checkpoint!");
            }
        }
        else
        {
            Debug.LogError("RespawnPoint object is not assigned!");
        }
    }
}
