using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class obstacle_camera_Manager : MonoBehaviour
{
    public float moveSpeed = 100.0f; // Cube의 이동 속도
    public Button forwardButton;
    public Button backwardButton;
    public Button rightButton;
    public Button leftButton;
    public Button zoomInButton;
    public Button zoomOutButton;
    public GameObject cubeObject;

    public bool IS_FORWARD;
    public bool IS_BACKWARD;
    public bool IS_RIGHT;
    public bool IS_LEFT;
    public bool IS_ZOOM_IN;
    public bool IS_ZOOM_OUT;
    public void Start()
    {
        forwardButton = GameObject.Find("Plane/Canvas/UpButton").GetComponent<Button>();
        backwardButton = GameObject.Find("Plane/Canvas/DownButton").GetComponent<Button>();
        leftButton = GameObject.Find("Plane/Canvas/LeftButton").GetComponent<Button>();
        rightButton = GameObject.Find("Plane/Canvas/RightButton").GetComponent<Button>();
        zoomInButton = GameObject.Find("Plane/Canvas/ExtensionButton").GetComponent<Button>();
        zoomOutButton = GameObject.Find("Plane/Canvas/ReductionButton").GetComponent<Button>();
        cubeObject = GameObject.Find("Cube");
        IS_FORWARD = false;
        IS_BACKWARD = false;
        IS_RIGHT = false;
        IS_LEFT = false;
        IS_ZOOM_IN = false;
        IS_ZOOM_OUT = false;
    }
    public void Update()
    {
        Debug.Log("state:" + IS_FORWARD);
        if (IS_FORWARD)
        {
            Debug.Log("FORWARD");
            cubeObject.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        else if (IS_BACKWARD)
        {
            cubeObject.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else if (IS_RIGHT)
        {
            cubeObject.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else if (IS_LEFT)
        {
            cubeObject.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else if (IS_ZOOM_IN)
        {
            cubeObject.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
        else if (IS_ZOOM_OUT)
        {
            cubeObject.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
    }
    public void FORWARD_DOWN()
    {
        IS_FORWARD = true;
        Debug.Log("Forward down");
        Debug.Log(IS_FORWARD);
        GameObject.Find("Cube").transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
    }
    public void FORWARD_UP()
    {
        IS_FORWARD = false;
        Debug.Log("Forward up");
        Debug.Log(IS_FORWARD);
    }
    public void BACKWARD_DOWN()
    {
        IS_BACKWARD = true;
        GameObject.Find("Cube").transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
    public void BACKWARD_UP()
    {
        IS_BACKWARD = false;
    }
    public void RIGHT_DOWN()
    {
        IS_RIGHT = true;
        GameObject.Find("Cube").transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }
    public void RIGHT_UP()
    {
        IS_RIGHT = false;
    }
    public void LEFT_DOWN()
    {
        IS_LEFT = true;
        GameObject.Find("Cube").transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
    public void LEFT_UP()
    {
        IS_LEFT = false;
    }



    public void ZOOMIN_DOWN()
    {
        IS_ZOOM_IN = true;
        GameObject.Find("Cube").transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }
    public void ZOOMIN_UP()
    {
        IS_ZOOM_IN = false;
    }



    public void ZOOMOUT_DOWN()
    {
        IS_ZOOM_OUT = true;
        GameObject.Find("Cube").transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }
    public void ZOOMOUT_UP()
    {
        IS_ZOOM_OUT = false;
    }
}
