using UnityEngine;

public class PlayableAreaEnforcer : PlayerTeleporterBase
{
    // respawn the player if they leave the area of the object
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))  // check if the colliding object is the Player
        {
            TeleportPlayer(other.gameObject);
        }
    }
}
