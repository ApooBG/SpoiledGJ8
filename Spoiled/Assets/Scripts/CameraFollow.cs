using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;          // The target object that the camera will follow
    private Vector3 offsetPosition;   // The initial offset between the camera and the target

    void Start()
    {
        // Calculate the initial offset between the camera's position and the target's position
        offsetPosition = transform.position - target.position;

    }

    void Update()
    {
        // Update the camera's position to follow the target while maintaining the initial offset
        transform.position = target.position + offsetPosition;
        Debug.Log("Object: " + target.position);
        Debug.Log("Camera: " + transform.position);

    }
}
