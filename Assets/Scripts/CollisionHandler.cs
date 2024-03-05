using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public float damageAmount = 10;
    
    // add damage on game object
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHealth health))
        {
            health.DecreaseHealth(damageAmount);
        }
    }

    // add damage on trigger 
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerHealth health))
        {
            health.DecreaseHealth(damageAmount);
        }
    }
}
