using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using UnityEngine.UI;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using UnityEngine.AI;

public class DatabaseSearch : MonoBehaviour
{
    public InputField searchInputField; // ����Ƽ���� �Է� �ʵ带 �Ҵ��ؾ� �մϴ�.
    public Button searchButton; // ����Ƽ���� ��ư�� �Ҵ��ؾ� �մϴ�.
    public GameObject planeObject;
    public GameObject cubePrefab;
    string searchText;
    public static float node_x;
    public static float node_y;
    public static float scale_x;
    public static float scale_y;
    public static float plane_w;
    public static float plane_h;
    private TargetChanger targetChanger;
    public Camera otherCamera;
    public const string MONGODB_URI_FORMAT = "mongodb://OS:MZWl4yS6ylx53ouQ@atlas-sql-6560a86c6cf7c44c1c5f5f3c-tbnaj.a.query.mongodb.net/test?ssl=true&authSource=admin";
    private const string TEST_DB = "test";
    private const string TEST_COLLECTION = "destination_position";
    public string collectionName = "obstacles";
    public int des_ID;

    public MongoClient client;
    public IMongoDatabase database;
    public IMongoCollection<TestData> testCollection;

    public GameObject real_player_ar;
    public NavMeshAgent target_player;

    public ListDatabaseText textarray_object;
    public float des_x;
    public float des_y;
    private void Start()
    {
        searchText = null;
        GetImageSize();
        targetChanger = FindObjectOfType<TargetChanger>();
        otherCamera = GameObject.Find("ScaleCamera").GetComponent<Camera>();
        if (otherCamera == null)
        {
            Debug.LogError("Other camera not found!");
        }
        
        client = new MongoClient(MONGODB_URI_FORMAT);
        database = client.GetDatabase(TEST_DB);
        testCollection = database.GetCollection<TestData>(TEST_COLLECTION);

        var filter = Builders<TestData>.Filter.Empty;
        var result = testCollection.Find(filter).ToList();
        foreach (var data in result)
        {
            textarray_object.des_name_list.Add(data.Text);
        }
    }
    public void SearchDatabase()
    {
        Vector3 startPosition = target_player.transform.position;
        Vector3 offMeshLinkDestination = new Vector3(real_player_ar.transform.position.x, 0.1f, real_player_ar.transform.position.z);
        target_player.Warp(offMeshLinkDestination);

        searchText = searchInputField.text; //�ؽ�Ʈ ���̰� 3�̻��� �ƴ϶�� �ٽ� �Է¹޵��� �ϴ°� �߰��ϱ�
        Debug.Log(searchText);

        //client = new MongoClient(MONGODB_URI_FORMAT);
        Debug.Log("Got Client");
        //database = client.GetDatabase(TEST_DB);
        //testCollection = database.GetCollection<TestData>(TEST_COLLECTION);
        //files = new GridFSBucket(database, new GridFSBucketOptions { BucketName = TEST_FS });
        Debug.Log("good");

        try
        {
            var filter = Builders<TestData>.Filter.Regex("Text", new BsonRegularExpression(searchText, "i"));
            var result = testCollection.Find(filter).ToList();

            if (result.Count > 0)
            {
                float xx=0, yy=0;
                int res_des_ID = 0;
                foreach (var document in result)
                {
                    Debug.Log("�˻� ���: " + document.ID + "->" + document.Text + " (" + document.Node_X + ", " + document.Node_Y + ")");
                    res_des_ID = document.ID;
                    xx = document.Node_X;
                    yy = document.Node_Y;
                }
                des_ID = res_des_ID;
                node_x = xx;
                node_y = yy;
                set_destination_pos(); // �˻� ����� ���� �� ó���� ���� �߰�
            }
            else
            {
                Debug.Log("�������� �ٽ� �Է����ּ���.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("MongoDB ���� �Ǵ� �˻� ����: " + ex.Message);
        }
    }
    void GetImageSize()
    {
        Renderer renderer = planeObject.GetComponent<Renderer>();
        Texture texture = renderer.material.mainTexture;
        if (texture is Texture2D)
        {
            Texture2D texture2D = (Texture2D)texture;
            int imageWidth = texture2D.width;
            int imageHeight = texture2D.height;
            float planeWidth = renderer.bounds.size.x;
            float planeHeight = renderer.bounds.size.z;
            plane_w = planeWidth;
            plane_h = planeHeight;
            Debug.Log("�̹���: (" + imageWidth + "," + imageHeight + ")");
            Debug.Log("�÷���: (" + planeWidth + "," + planeHeight + ")");

            scale_x = planeWidth / imageWidth;
            scale_y = planeHeight / imageHeight;

            Debug.Log("������ ����: " + scale_x + ", ������ ����: " + scale_y);
        }
        else
        {
            Debug.LogError("����X");
        }
    }
    void set_destination_pos()
    {
        float clone_x = node_x;
        float clone_y = node_y;

        float plane_pos_x = clone_x * scale_x;
        float plane_pos_y = clone_y * scale_y;

        plane_pos_x = plane_w - plane_pos_x;
        //plane_pos_y = plane_h - plane_pos_y;

        des_x = plane_pos_x;
        des_y = plane_pos_y;

        //Vector3 pos1 = new Vector3(plane_pos_x, 0f, plane_pos_y);
        //GameObject cube1 = Instantiate(cubePrefab, pos1, Quaternion.identity);

        Debug.Log("plane_pos_x: " + plane_pos_x + ", plane_pos_y: " + plane_pos_y);
        targetChanger.destination_pos(1, plane_pos_x, plane_pos_y);
    }
}