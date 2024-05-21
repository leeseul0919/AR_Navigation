using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using WebSocketSharp;
using TMPro;

public class obs_type
{
    public int obs_id;
    public GameObject obs;
    public Vector3 start;
    public Vector3 end;
    public obs_type(int id, GameObject cb, Vector3 start_data, Vector3 end_data)
    {
        obs_id = id;
        obs = cb;
        start = start_data;
        end = end_data;
    }
} 
public class CubeCreator : MonoBehaviour
{
    public Camera obstacleCamera;
    public GameObject cubePrefab;
    public Button createButton;

    private Vector3 startPos;
    private bool isStartSet = false;
    public bool iscreate = false;

    public WebSocket webSocket;

    public Transform obs_content;
    public GameObject obs_clone;

    public List<obs_type> Obstacles = new List<obs_type>();
    public int progress_st;
    public void Start()
    {
        createButton.onClick.AddListener(OnCreateButton);
        webSocket = new WebSocket("wss://port-0-docker-node-be-1ru12mlvsjr55i.sel5.cloudtype.app/:3000");
        webSocket.OnClose += OnClose;
    }
    public void update_obs_list()
    {
        Debug.Log("update_obs_list");
        foreach (Transform child in obs_content)
        {
            Destroy(child.gameObject);
        }
        int i = 1;
        foreach (obs_type obstacle in Obstacles)
        {
            GameObject add_obs_clone = Instantiate(obs_clone, obs_content);
            TextMeshProUGUI obs_text = add_obs_clone.GetComponentInChildren<TextMeshProUGUI>();
            obs_text.text = $"({i}), {obstacle.obs.transform.position.x}, {obstacle.obs.transform.position.y}, {obstacle.obs.transform.position.z}";
            Button obs_delete_button = add_obs_clone.GetComponentInChildren<Button>();
            obs_delete_button.onClick.AddListener(() => DeleteObstacle(obstacle, add_obs_clone));
            i++;
        }
    }

    public void DeleteObstacle(obs_type obstacle, GameObject clone)
    {
        // 게임 오브젝트 리스트에서 삭제
        Obstacles.Remove(obstacle);
        Destroy(obstacle.obs);
        // UI에서 해당 항목 삭제
        Destroy(clone);
        webSocket.Connect();
        progress_st = 3;
        webSocket.Send($"{{\"progress_st\": \"{progress_st}\", \"obs_id\": \"{obstacle.obs_id}\", \"start_x\": \"{obstacle.start.x}\", \"start_z\": \"{obstacle.start.z}\", \"end_x\": \"{obstacle.end.x}\", \"end_z\": \"{obstacle.end.z}\"}}");
        update_obs_list();
    }
    public void OnCreateButton()
    {
        iscreate = true;
    }
    void Update()
    {
        if(iscreate == true)
        {
            // 화면에 터치 입력이 있는지 확인
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0); // 첫 번째 터치를 가져옴

                // 터치가 시작됐을 때
                if (touch.phase == TouchPhase.Began)
                {
                    // 시작점이 설정되지 않았다면, 시작점을 설정
                    if (!isStartSet)
                    {
                        startPos = GetWorldPosition(touch.position);
                        Debug.Log("시작 좌표 (스크린): " + touch.position + ", 시작 좌표 (월드): " + startPos);
                        isStartSet = true;
                    }
                    else // 시작점이 이미 설정된 상태라면, 종료점을 설정하고 큐브를 생성
                    {
                        Vector3 endPos = GetWorldPosition(touch.position);
                        Debug.Log("종료 좌표 (스크린): " + touch.position + ", 종료 좌표 (월드): " + endPos);
                        CreateCube(startPos, endPos, Obstacles.Count + 1);

                        isStartSet = false; // 다음 입력을 위해 시작점 설정을 초기화
                        iscreate = false;
                    }
                }
            }
        }
    }

    // 스크린 좌표를 월드 좌표로 변환하는 함수
    Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        Ray ray = obstacleCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    // 두 점 사이에 큐브를 생성하는 함수
    public void CreateCube(Vector3 start, Vector3 end, int obs_id)
    {
        // 시작점과 종료점을 각각 대각선의 양 끝점으로 설정
        Vector3 topLeft = new Vector3(Mathf.Min(start.x, end.x), Mathf.Max(start.y, end.y), Mathf.Min(start.z, end.z));
        Vector3 bottomRight = new Vector3(Mathf.Max(start.x, end.x), Mathf.Min(start.y, end.y), Mathf.Max(start.z, end.z));

        // 대각선의 중심점을 구함
        Vector3 middlePoint = (topLeft + bottomRight) / 2;

        // 큐브 생성
        Vector3 cubePosition = new Vector3(middlePoint.x, 0.2f, middlePoint.z); // y 좌표를 고정
        GameObject cube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
        cube.transform.localScale = new Vector3(Mathf.Abs(start.x - end.x), 5.123f, Mathf.Abs(start.z - end.z));

        Debug.Log("CubeCreator >> 큐브 생성 좌표 (월드): " + cubePosition);
        obs_type new_obs = new obs_type(obs_id, cube, start, end);
        Obstacles.Add(new_obs);
        webSocket.Connect();
        progress_st = 2;
        webSocket.Send($"{{\"progress_st\": \"{progress_st}\", \"obs_id\": \"{obs_id}\", \"start_x\": \"{start.x}\", \"start_z\": \"{start.z}\", \"end_x\": \"{end.x}\", \"end_z\": \"{end.z}\"}}");
        update_obs_list();
    }


    public void OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("CubeCreator WebSocket connection closed.");
    }

    private void OnApplicationQuit()
    {
        // 애플리케이션 종료 시 WebSocket 연결 종료
        if (webSocket != null && webSocket.IsAlive)
        {
            webSocket.Close();
        }
    }
}
