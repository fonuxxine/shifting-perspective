using System;
using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    
    public float openAngle = 60f;
    public float closeAngle = 0f;
    public float rotationSpeed = 30f;
    public float detectionRange = 2f;
    
    private bool _isOpen = false;
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (!_isOpen)
            {
                StartCoroutine(OpenDoor());
                _isOpen = true;
            }
        }
        else
        {
            if (_isOpen)
            {
                StartCoroutine(CloseDoor());
                _isOpen = false;
            }
        }
    }

    IEnumerator OpenDoor()
    {
        while (Mathf.Abs(transform.localRotation.eulerAngles.y - openAngle) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, openAngle, 0), step);
            yield return null;
        }
    }

    IEnumerator CloseDoor()
    {
        float currentAngle = transform.localRotation.eulerAngles.y;
        float targetAngle = Mathf.Repeat(closeAngle, 360f); // normalize the closeAngle

        while (Mathf.Abs(currentAngle - targetAngle) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime;
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, step);
            transform.localRotation = Quaternion.Euler(0, currentAngle, 0);
            yield return null;
        }
    }


}