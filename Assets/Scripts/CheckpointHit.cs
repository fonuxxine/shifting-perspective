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

                        // make the RespawnPoint a child of the checkpoint at the position of RespawnPlaceholder
                        MakeRespawnPointChild(hitCollider.gameObject);

                        // activate the child checkpoint flame & sound
                        ActivateChildCheckpointFlame(hitCollider.gameObject);
                        
                        // update the base rotation of the level (for re-rotation after respawn)
                        rotate rotationScript = GetComponentInParent<rotate>();
                        rotationScript.UpdateBaseAngles(respawnPoint.transform.localRotation);
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

    // make the RespawnPoint a child of the checkpoint at the position of RespawnPlaceholder
    private void MakeRespawnPointChild(GameObject checkpoint)
    {
        if (respawnPoint != null)
        {
            Transform respawnPlaceholder = checkpoint.transform.Find("RespawnPlaceholder");
            if (respawnPlaceholder != null)
            {
                respawnPoint.transform.parent = checkpoint.transform;
                respawnPoint.transform.SetLocalPositionAndRotation(respawnPlaceholder.localPosition, respawnPlaceholder.localRotation);
                // respawnPoint.transform.SetPositionAndRotation(respawnPlaceholder.position, respawnPlaceholder.rotation);
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
    
    // activate the child checkpoint flame
    private void ActivateChildCheckpointFlame(GameObject checkpoint)
    {
        Transform childCheckpointFlame = checkpoint.transform.Find("CheckpointFlame");
        if (childCheckpointFlame != null)
        {
            childCheckpointFlame.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No CheckpointFlame found as child of the checkpoint!");
        }
    }
}
