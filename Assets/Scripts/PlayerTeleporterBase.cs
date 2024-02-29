using UnityEngine;
using System.Collections;

public abstract class PlayerTeleporterBase : MonoBehaviour
{
    public Transform spawnPoint;
    private CharacterController _characterController;

    protected virtual void Start()
    {
        _characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    protected void TeleportPlayer(GameObject player)
    {
        // rotate the level back to its original position.
        rotate rotationScript = player.GetComponentInParent<rotate>();
        if (rotationScript != null)
        {
            // reset rotation and teleport player
            StartCoroutine(TeleportAfterRotation(rotationScript, player));
        }
        else
        {
            Debug.LogError("Rotate script not found in the player's parent.");
        }
    }

    private IEnumerator TeleportAfterRotation(rotate rotationScript, GameObject player)
    {
        // teleport player to spawn point position.
        player.SetActive(false);
        player.transform.position = spawnPoint.position;
        
        
        // wait until the level rotation coroutine is completed.
        yield return StartCoroutine(rotationScript.ResetRotation());

        // disable the CharacterController to prevent movement during teleportation.
        _characterController.enabled = false;

        player.SetActive(true);

        // orient the player's feet downward.
        Vector3 newRotation = Quaternion.FromToRotation(Vector3.up, -spawnPoint.up).eulerAngles;
        player.transform.rotation = Quaternion.Euler(0f, newRotation.y, 0f);

        // re-enable the CharacterController after teleportation.
        _characterController.enabled = true;
    }
}
