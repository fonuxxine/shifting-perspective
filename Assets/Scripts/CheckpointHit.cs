using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHit : MonoBehaviour
{
    public ProgressManager progressManager;
    
    // Start is called before the first frame update
    void Start()
    {
        progressManager = FindObjectOfType<ProgressManager>();
    }
    
    // Update the saved current stage
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            progressManager.UpdateProgress();
        }
    }
}
