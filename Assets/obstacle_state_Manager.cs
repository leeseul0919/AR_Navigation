using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using UnityEngine.UI;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public class obstacle_state_Manager : MonoBehaviour
{
    public string mongodbURI = "mongodb://OS:MZWl4yS6ylx53ouQ@atlas-sql-6560a86c6cf7c44c1c5f5f3c-tbnaj.a.query.mongodb.net/test?ssl=true&authSource=admin";
    public string dbName = "test";
    public string collectionName = "obstacles";

    public MongoClient client;
    public IMongoDatabase database;
    public IMongoCollection<BsonDocument> collection;

    public GameObject cubePrefab;

    public CubeCreator obstacle_list;
    void Start()
    {
        client = new MongoClient(mongodbURI);
        database = client.GetDatabase(dbName);
        collection = database.GetCollection<BsonDocument>(collectionName);

        var filter = Builders<BsonDocument>.Filter.Empty;
        var cursor = collection.FindSync(filter);
        var documents = cursor.ToList();
        
        // 가져온 데이터 처리
        foreach (var doc in documents)
        {
            // 데이터 처리 코드 작성
            int obsId = doc["obs_id"].AsInt32;
            double startX = doc["start_x"].AsDouble;
            //double startY = 0.1; // 높이 설정
            double startZ = doc["start_z"].AsDouble;
            double endX = doc["end_x"].AsDouble;
            //double endY = 2.0; // 높이 설정
            double endZ = doc["end_z"].AsDouble;

            Vector3 start = new Vector3((float)startX, 0, (float)startZ);
            Vector3 end = new Vector3((float)endX, 0, (float)endZ);
            CreateCube(start, end, obsId);
        }
        obstacle_list.update_obs_list();
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

        Debug.Log("obstacle_state_Manage >> 큐브 생성 좌표 (월드): " + cubePosition);
        obs_type new_obs = new obs_type(obs_id, cube, start, end);
        obstacle_list.Obstacles.Add(new_obs);
    }
}
