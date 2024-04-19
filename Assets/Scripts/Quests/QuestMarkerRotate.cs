using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarkerRotate : MonoBehaviour
{
    private GameObject _questMarker;

    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _questMarker = gameObject;
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion cameraRot = _camera.transform.rotation;
        _questMarker.transform.LookAt(_questMarker.transform.position + cameraRot * Vector3.forward, cameraRot * Vector3.up);
    }
}
