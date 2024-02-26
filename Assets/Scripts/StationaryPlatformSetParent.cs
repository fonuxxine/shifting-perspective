using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryPlatformSetParent : MonoBehaviour
{
    public string desiredParentName = "Level_1"; // Public variable for setting the parent name in the Inspector

    // Detach player parent when entering the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // check if the colliding object is the Player
        {
            other.transform.parent = null;
        }
    }
    
    // Attach player parent when exiting the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))  // check if the colliding object is the Player
        {
            // Find the desired parent by name
            Transform desiredParent = GameObject.Find(desiredParentName)?.transform;
            other.transform.parent = desiredParent;
        }
    }
}