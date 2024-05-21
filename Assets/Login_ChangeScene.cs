using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using TMPro;

public class Login_ChangeScene : MonoBehaviour
{

    public MongoClient client;
    public IMongoDatabase userDatabase;
    public IMongoCollection<BsonDocument> userCollection;

    private const string USER_MONGODB_URI_FORMAT = "mongodb://OS:MZWl4yS6ylx53ouQ@atlas-sql-6560a86c6cf7c44c1c5f5f3c-tbnaj.a.query.mongodb.net/test?ssl=true&authSource=admin";
    private const string USER_DATABASE_NAME = "test";
    private const string USER_COLLECTION_NAME = "users";

    public InputField idInputField;
    public InputField passwordInputField;
    public string nickname;
    public string password;
    public TextMeshProUGUI messageText;
    public void Change()
    {
        // MongoDB 연결 설정
        client = new MongoClient(USER_MONGODB_URI_FORMAT);
        userDatabase = client.GetDatabase(USER_DATABASE_NAME);
        userCollection = userDatabase.GetCollection<BsonDocument>(USER_COLLECTION_NAME);

        string nickname = idInputField.text;
        string password = passwordInputField.text;

        // MongoDB에서 사용자 정보 확인
        var filter = Builders<BsonDocument>.Filter.Eq("ID", nickname) & Builders<BsonDocument>.Filter.Eq("Password", password);
        var result = userCollection.Find(filter).FirstOrDefault();
        Debug.Log(result);
        Debug.Log(userCollection.Find(filter).FirstOrDefault());

        if (result != null)
        {
            // Manager_check 필드의 값이 정수형인지 확인하고 정수로 변환
            int managerCheck = result.GetValue("Manager_check").AsInt32;

            // 로그인 성공 시 다음 씬으로 이동
            PlayerPrefs.SetString("Nick", nickname);
            PlayerPrefs.SetInt("ManagerCheck", managerCheck); // Manager_check 데이터를 PlayerPrefs에 저장
            Debug.Log("로그인 성공! 사용자 이름: " + nickname);
            SceneManager.LoadScene("menu");
        }
        else
        {
            // 로그인 실패 시 메시지 출력
            Debug.Log("로그인 실패: 사용자 이름 또는 비밀번호가 일치하지 않습니다.");
            messageText.text = "로그인 실패: 사용자 이름 또는 비밀번호가 일치하지 않습니다.";
            StartCoroutine(ClearMessageText(2f));
        }


        GameObject.Find("LoginCanvas/InputFieldID").GetComponent<InputField>().text = "";
        GameObject.Find("LoginCanvas/InputFieldPS").GetComponent<InputField>().text = "";

        // MongoDB 클라이언트 종료
        client = null;
        userDatabase = null;
        userCollection = null;
    }

    IEnumerator ClearMessageText(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageText.text = "";
    }

}