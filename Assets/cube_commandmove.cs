using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube_commandmove : MonoBehaviour
{
    private Rigidbody playerRigidbody;   // 이동에 사용할 리지드바디 컴포넌트
    public float speed = 8f;             //이동 속력

    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log("update");
        if (Input.GetKey(KeyCode.UpArrow) == true)
        {
            Debug.Log("up");
            playerRigidbody.AddForce(0f, 0f, speed);
        }
        if (Input.GetKey(KeyCode.DownArrow) == true)
        {
            Debug.Log("down");
            playerRigidbody.AddForce(0f, 0f, -speed);
        }
        if (Input.GetKey(KeyCode.RightArrow) == true)
        {
            Debug.Log("right");
            playerRigidbody.AddForce(speed, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.LeftArrow) == true)
        {
            Debug.Log("left");
            playerRigidbody.AddForce(-speed, 0f, 0f);
        }
    }
}
