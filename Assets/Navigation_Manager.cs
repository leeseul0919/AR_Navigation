using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Vuforia;
using System.IO;
using WebSocketSharp;
using Newtonsoft.Json;
using TMPro;

public class Navigation_Manager : MonoBehaviour
{
    public Text navigation_text;
    public string nickname;
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    public GameObject minimap;
    public GameObject idpanel;
    public GameObject searchinput;
    public GameObject searchbutton;
    public GameObject createbutton;
    public GameObject firsttext;
    public GameObject obstacle_manager;
    public GameObject real_player_ar;
    public GameObject search_list;
    public GameObject Obstacle_list;
    public GameObject ExtensionButton;
    public GameObject ReductionButton;
    public Text obstacle_st_text;
    public int st1;

    public Camera targetCamera;
    public int priority = 2;

    public WebSocket webSocket;

    public SimpleCloudRecoEventHandler vuforia_searching;
    
    public GameObject cubePrefab;
    public bool obs_edit = false;
    public int edit_st = 0;
    public Vector3 start_pos;
    public Vector3 end_pos;
    public ObstacleData obstacle;

    public Queue<ObstacleData> messageQueue = new Queue<ObstacleData>();
    public CubeCreator obs_list_manage;
    public DatabaseSearch pathfinder_save;
    public TargetChanger targetchanger;
    void Start()
    {
        nickname = PlayerPrefs.GetString("Nick");
        st1= PlayerPrefs.GetInt("ST");
        navigation_text.text = "ID: " + nickname;
        if(st1==1)
        {
            obstacle_st_text.text = "";
            minimap.SetActive(false);
            idpanel.SetActive(false);
            searchinput.SetActive(false);
            searchbutton.SetActive(false);
            firsttext.SetActive(false);
            search_list.SetActive(false);
            targetCamera.depth = priority;
        }
        else if(st1==2)
        {
            obstacle_st_text.text = "";
            Obstacle_list.SetActive(false);
            button1.SetActive(false);
            button2.SetActive(false);
            button3.SetActive(false);
            button4.SetActive(false);
            createbutton.SetActive(false);
            ExtensionButton.SetActive(false);
            ReductionButton.SetActive(false);
            obstacle_manager.SetActive(false);
            minimap.SetActive(false);
            idpanel.SetActive(false);
            searchinput.SetActive(false);
            searchbutton.SetActive(false);
            search_list.SetActive(false);
        }
    }
    public void start_positioin_set()
    {
        Vector3 newPosition;
        if (vuforia_searching.targetName.Equals("305"))
        {
            newPosition = new Vector3(19.59f, 1.0f, 144.87f);
            real_player_ar.transform.Rotate(0, -90, 0);
        }
        else if (vuforia_searching.targetName.Equals("352"))
        {
            newPosition = new Vector3(69.61f, 1.0f, 99.09f);
            real_player_ar.transform.Rotate(0, -90, 0);
        }
        else newPosition = new Vector3(68.95f, 1.0f, 78.79f);
        real_player_ar.transform.position = newPosition;
        Debug.Log("move start point");
        start_navigation();
    }
    public void start_navigation()
    {
        minimap.SetActive(true);
        idpanel.SetActive(true);
        searchinput.SetActive(true);
        searchbutton.SetActive(true);
        search_list.SetActive(true);

        webSocket = new WebSocket("wss://port-0-docker-node-be-1ru12mlvsjr55i.sel5.cloudtype.app/:3000");
        webSocket.OnMessage += OnMessage;
        webSocket.OnClose += OnClose;
        webSocket.Connect();

        StartCoroutine(CheckQueue());
    }
    IEnumerator CheckQueue()
    {
        WaitForSeconds waitSec = new WaitForSeconds(1);
        while(true)
        {
            if (messageQueue.Count > 0)
            {
                ObstacleData data = messageQueue.Dequeue();
                string st_text;
                if (data.st == 1)
                {
                    st_text = "create";
                    map_obstacle_create(data);
                }
                else
                {
                    st_text = "delete";
                    map_obstacle_delete(data);
                }
                obstacle_st_text.text = "data >> " + st_text + " obs_id: " + data.obs_id;
            }
            yield return waitSec;
        }
    }
    void map_obstacle_create(ObstacleData d)
    {
        Vector3 start_pos = new Vector3((float)d.start_x, 0.1f, (float)d.start_z);
        Vector3 end_pos = new Vector3((float)d.end_x, 0.1f, (float)d.end_z);
        CreateCube(start_pos, end_pos, d.obs_id);
        Debug.Log("find again");
        float clone_x = pathfinder_save.des_x;
        float clone_y = pathfinder_save.des_y;
        targetchanger.destination_pos(2, clone_x, clone_y);
    }
    void map_obstacle_delete(ObstacleData d)
    {
        for (int i = 0; i < obs_list_manage.Obstacles.Count; i++)
        {
            if(obs_list_manage.Obstacles[i].obs_id == d.obs_id)
            {
                Debug.Log("delete obs_id >>" + obs_list_manage.Obstacles[i].obs_id);
                GameObject.Destroy(obs_list_manage.Obstacles[i].obs);
                obs_list_manage.Obstacles.RemoveAt(i);
                break;
            }
        }
        Debug.Log("find again");
        float clone_x = pathfinder_save.des_x;
        float clone_y = pathfinder_save.des_y;
        targetchanger.destination_pos(3, clone_x, clone_y);
    }
    public class ObstacleData
    {
        public int st { get; set; }
        public int obs_id { get; set; }
        public double start_x { get; set; }
        public double start_z { get; set; }
        public double end_x { get; set; }
        public double end_z { get; set; }
    }
    public void OnMessage(object sender, MessageEventArgs e)
    { 
        var jsonString = e.Data;
        Debug.Log("sever send >>" + jsonString);
        obstacle = JsonConvert.DeserializeObject<ObstacleData>(jsonString);
        messageQueue.Enqueue(obstacle);
    }
    public void OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket connection closed.");
        if (webSocket != null && !webSocket.IsAlive)
        {
            webSocket.Connect();
        }
    }
    public void goto_menu()
    {
        SceneManager.LoadScene("menu");
    }
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

        obs_type new_obs = new obs_type(obs_id, cube, start, end);
        obs_list_manage.Obstacles.Add(new_obs);
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
