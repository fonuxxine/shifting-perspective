using UnityEngine;

public class CheckpointHit : MonoBehaviour
{
    // active checkpoint material
    public Material greenMaterial;

    // prior checkpoint material
    public Material yellowMaterial;

    // reference to the RespawnPoint GameObject
    public GameObject respawnPoint;

    // radius to detect the player
    public float detectionRadius = 3f;

    // variable to track the current checkpoint number
    private int currentCheckpoint = 0;

    // array of previously hit checkpoints
    private GameObject[] previousCheckpoints;

    void Start()
    {
        previousCheckpoints = new GameObject[0];
    }

    void Update()
    {
        // check if any objects with name "Checkpoint<int>" are within range
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
                        // change checkpoint material to green
                        foreach (var checkpoint in previousCheckpoints)
                        {
                            checkpoint.GetComponent<Renderer>().material = yellowMaterial;
                        }
                        renderer.material = greenMaterial;
                        currentCheckpoint = checkpointNumber;
                        
                        // update previously hit checkpoints array
                        UpdatePreviousCheckpoints(hitCollider.gameObject);

                        // move the RespawnPoint to the location of the child "RespawnPlaceholder" under the checkpoint
                        MoveRespawnPoint(hitCollider.gameObject.transform);

                        // update the base rotation of the level (for re-rotation after respawn)
                        rotate rotationScript = GetComponentInParent<rotate>();
                        rotationScript.UpdateBaseAngles(respawnPoint.transform.rotation);
                    }
                    else if (checkpointNumber < currentCheckpoint)
                    {
                        // change checkpoint material to yellow
                        renderer.material = yellowMaterial;
                        
                        // update previously hit checkpoints array
                        UpdatePreviousCheckpoints(hitCollider.gameObject);
                    }
                }
            }
        }
    }

    // visualize the detection radius in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // helper method to extract the checkpoint number from the GameObject name
    private int GetCheckpointNumber(string checkpointName)
    {
        string numberString = checkpointName.Substring("Checkpoint".Length);
        return int.Parse(numberString);
    }

    // update previously hit checkpoints array
    private void UpdatePreviousCheckpoints(GameObject newCheckpoint)
    {
        GameObject[] newPreviousCheckpoints = new GameObject[previousCheckpoints.Length + 1];
        for (int i = 0; i < previousCheckpoints.Length; i++)
        {
            newPreviousCheckpoints[i] = previousCheckpoints[i];
        }
        newPreviousCheckpoints[^1] = newCheckpoint;
        previousCheckpoints = newPreviousCheckpoints;
    }

    // move the RespawnPoint to the position of the checkpoint's "RespawnPlaceholder" child
    private void MoveRespawnPoint(Transform checkpointTransform)
    {
        if (respawnPoint != null)
        {
            Transform respawnPlaceholder = checkpointTransform.Find("RespawnPlaceholder");
            if (respawnPlaceholder != null)
            {
                respawnPoint.transform.position = respawnPlaceholder.position;
                respawnPoint.transform.rotation = respawnPlaceholder.rotation;
            }
            else
            {
                Debug.LogWarning("No RespawnPlaceholder found under the checkpoint!");
            }
        }
        else
        {
            Debug.LogError("RespawnPoint object is not assigned to checkpoint!");
        }
    }
}
