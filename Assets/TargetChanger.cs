using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetChanger : MonoBehaviour
{
    RaycastHit hit;
    public GameObject player;
    public Canvas m_canvas;
    float plane_w;
    public GameObject cubePrefab;
    public Camera otherCamera;
    public GameObject real_player_ar;
    public NavMeshAgent agent;

    private void Start()
    {
        float plane_w = DatabaseSearch.plane_w;
        otherCamera = GameObject.Find("ScaleCamera").GetComponent<Camera>();
        if (otherCamera == null)
        {
            Debug.LogError("Other camera not found!");
        }
    }

    //void Update()
    //{
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        Debug.Log("Ray origin: " + ray.origin);
    //        Debug.Log("Ray direction: " + ray.direction);
    //        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red);

    //        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    //        {
    //            Debug.Log("Click");
    //            this.transform.position = hit.point;
    //            Debug.Log(this.transform.position.x);
    //            Debug.Log(this.transform.position.y);
    //            Debug.Log(this.transform.position.z);
    //            player.GetComponent<PathFinder>().makePath();
    //        }
    //    }
    //}
    public void destination_pos(int st, float x, float y)
    {
        Vector3 offMeshLinkDestination = new Vector3(real_player_ar.transform.position.x, 0.1f, real_player_ar.transform.position.z);
        agent.Warp(offMeshLinkDestination);
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        float clone_x = x;
        float clone_y = y;

        // 큐브를 생성하고 해당 큐브의 위치를 받아옵니다.
        Vector3 targetPosition = new Vector3(clone_x, 0.5f, clone_y); // 타겟의 위치
        Debug.Log("real targetPosition >> " + targetPosition);
        this.transform.position = targetPosition;
        player.GetComponent<PathFinder>().makePath();
    }
}