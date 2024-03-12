using UnityEngine;
using System.Collections;

// Killbind script to help debugging
public class Killbind : PlayerTeleporterBase
{
    public GameObject player;
    private bool canRespawn = true; // flag to track if the player can killbind

    // Update is called once per frame
    void Update()
    {
        // check if the 'K' key is pressed and if the killbind is allowed
        if (Input.GetKeyDown(KeyCode.K) && canRespawn)
        {
            // respawn the player
            StartCoroutine(RespawnCooldown());
        }
    }

    // coroutine to handle killbind cooldown
    IEnumerator RespawnCooldown()
    {
        // disable respawn for the cooldown duration
        canRespawn = false;

        // respawn the player
        TeleportPlayer(player);

        // wait for the cooldown duration
        yield return new WaitForSeconds(3f);

        // enable respawn after cooldown
        canRespawn = true;
    }
}
