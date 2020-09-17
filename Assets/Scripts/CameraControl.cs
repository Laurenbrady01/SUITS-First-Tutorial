using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float mouseSensitivity = 1;
    public float cameraSpeed = 10;
    public float clampAngle = 80;

    public GameObject ballPrefab;

    // FixedUpdate for the move
    void Update()
    {
        // Grab inputs first
        // Keyboard 
        Vector3 translation = new Vector3(0, 0, 0);

        translation.z = Input.GetAxisRaw("Vertical");
        translation.x = Input.GetAxisRaw("Horizontal");
        translation.y = Input.GetAxisRaw("Jump");

        // Mouse 
        float horz = 0;
        float vert = 0;


        horz = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        vert = -Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        //  Relative translation because it depends on the normal
        translation *= Time.deltaTime * cameraSpeed;
        transform.Translate(translation);

        // Rotation
        if (Cursor.lockState != CursorLockMode.None)
        {
            transform.RotateAround(transform.position, Vector3.up, horz);
            Vector3 rot = transform.localRotation.eulerAngles;
            rot.x = (transform.localRotation.eulerAngles.x + 180) % 360 - 180 + vert;
            rot.x = Mathf.Clamp(rot.x, -clampAngle, clampAngle);
            transform.localRotation = Quaternion.Euler(rot);
        }


        // Mouse control
        if (Input.GetMouseButton(1)) // Right click
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        // Mouse control
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            // Add stuff here
            UnityEngine.Debug.Log("I pressed mouse 1");

            Ray testRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool didWeHitSomething = Physics.Raycast(testRay.origin, testRay.direction, out hit);

            if (didWeHitSomething)
            {
                GameObject tempBall = GameObject.Instantiate(ballPrefab);
                Collider ballCollider = tempBall.GetComponent<Collider>();

                tempBall.transform.position = hit.point + (hit.normal * ballCollider.bounds.extents.y);
                tempBall.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            }
            UnityEngine.Debug.DrawLine(testRay.origin, testRay.origin + testRay.direction * 10, Color.red, 2);

            

           
        }
    }
}
