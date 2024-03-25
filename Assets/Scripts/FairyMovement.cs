using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = System.Numerics.Quaternion;

public class FairyMovement : MonoBehaviour
{
    public GameObject camera;
    public GameObject fairyCamera;
    public GameObject player;
    public GameObject level;
    public float speed = 5f;
    public bool isFairyPerspective = false;
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isFairyPerspective = !isFairyPerspective;
            if (isFairyPerspective)
            {
                camera.SetActive(false);
                fairyCamera.SetActive(true);
                player.GetComponent<MovementController>().enabled = false;
                level.GetComponent<rotate>().enabled = false;
            }
            else
            {
                camera.SetActive(true);
                fairyCamera.SetActive(false);
                player.GetComponent<MovementController>().enabled = true;
                level.GetComponent<rotate>().enabled = true;
            }
        }
        if (isFairyPerspective)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float ascend = Input.GetAxis("Jump");
            //move the fairy
            transform.Translate(new Vector3(0, 0, vertical) * speed * Time.deltaTime);
            transform.Translate(new Vector3(0, ascend, 0) * speed * Time.deltaTime);
            float rotationAmount = horizontal * 30f * speed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAmount);
        }
    }
}
