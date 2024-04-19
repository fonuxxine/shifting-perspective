using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryPlatformSetParent : MonoBehaviour
{
    public string desiredParentName = "Level_1"; // Public variable for setting the parent name in the Inspector
    public GameObject offEffect;
    public GameObject onEffect;
    public GameObject firstTimeDialogue;

    private bool _firstTime = true;

    // Detach player parent when entering the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // check if the colliding object is the Player
        {
            offEffect.GetComponent<ParticleSystem>().Stop();
            onEffect.GetComponent<ParticleSystem>().Play();
            other.transform.parent = null;

            if (_firstTime && firstTimeDialogue)
            {
                firstTimeDialogue.SetActive(true);
                _firstTime = false;
            }
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
            onEffect.GetComponent<ParticleSystem>().Stop();
            offEffect.GetComponent<ParticleSystem>().Play();
        }
        
    }
}