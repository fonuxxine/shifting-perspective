using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public Transform spawnPoint; // the spawn point marker
    private CharacterController characterController; // the CharacterController of the player

    private void Start()
    {
        // get the CharacterController component attached to the player GameObject
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // check if the colliding object is the Player
        {
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
