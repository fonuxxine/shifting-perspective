using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    private int stage;
    // Start is called before the first frame update
    void Start()
    {
        stage = 0;
    }

    public void UpdateProgress()
    {
        stage += 1;
    }
}
