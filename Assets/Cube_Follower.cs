using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Follower : MonoBehaviour
{
    public Transform arCameraTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = arCameraTransform.position;
        transform.rotation = arCameraTransform.rotation;
    }
}