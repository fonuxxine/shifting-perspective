using UnityEngine;

public abstract class PlayerTeleporterBase : MonoBehaviour
{
    public Transform spawnPoint;
    protected CharacterController characterController;

    protected virtual void Start()
    {
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    protected void TeleportPlayer(GameObject player)
    {
        // rotate the level back to its original position.
        rotate rotationScript = player.GetComponentInParent<rotate>();
        if (rotationScript != null)
        {
            rotationScript.ResetRotation();
        }
        else
        {
            Debug.LogError("Rotate script not found in the player's parent.");
        }

        // disable the CharacterController to prevent movement during teleportation.
        characterController.enabled = false;

        // teleport player to spawn point position.
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;

        // re-enable the CharacterController after teleportation.
        characterController.enabled = true;
    }
}
