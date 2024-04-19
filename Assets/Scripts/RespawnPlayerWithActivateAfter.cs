using UnityEngine;

public class RespawnPlayerWithActivateAfter : PlayerTeleporterBase
{
    public GameObject activateAfterOnce;

    private bool _activate = true;
    
    // respawn the player if they enter the object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // check if the colliding object is the Player
        {
            TeleportPlayer(other.gameObject);
            
            if (_activate)
            {
                _activate = false;
                activateAfterOnce.SetActive(true);
            }
        }
    }
}
