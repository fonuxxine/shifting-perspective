using UnityEngine;

public class RespawnPlayer : PlayerTeleporterBase
{
    // respawn the player if they enter the object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // check if the colliding object is the Player
        {
            TeleportPlayer(other.gameObject);
        }
    }
}
