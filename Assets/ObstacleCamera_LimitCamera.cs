using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCamera_LimitCamera : MonoBehaviour
{
    public GameObject Player;
    private void LateUpdate()
    {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
    }
}
