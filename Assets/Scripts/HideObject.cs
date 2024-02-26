using UnityEngine;

public class HideObject : MonoBehaviour
{
    // disable the bound object immediately
    void Start()
    {
        gameObject.SetActive(false);
        
    }
}
