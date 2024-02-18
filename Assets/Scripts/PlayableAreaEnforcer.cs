using UnityEngine;

public class PlayableAreaEnforcer : MonoBehaviour
{
    public Transform spawnPoint; // the spawn point marker
    private CharacterController characterController; // the CharacterController of the player

    private void Start()
    {
        // get the CharacterController component attached to the player GameObject
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // check if the colliding object is the Player
        {
            // respawn the player when they leave the trigger area
            TeleportPlayer(other.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        // disable the CharacterController to prevent movement during teleportation
        characterController.enabled = false;

        // teleport player to spawn point position
        player.transform.position = spawnPoint.position; 
        player.transform.rotation = spawnPoint.rotation;

        // re-enable the CharacterController after teleportation
        characterController.enabled = true;
    }
}
