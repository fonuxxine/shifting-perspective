using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformAnimations : MonoBehaviour
{
    public GameObject offEffect;
    public GameObject onEffect;
    // Start is called before the first frame update
    void Start()
    {
        offEffect.GetComponent<ParticleSystem>().Play();
        onEffect.GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
