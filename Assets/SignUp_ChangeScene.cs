using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using WebSocketSharp;

public class SignUp_ChangeScene : MonoBehaviour
{
    public string nickname;
    public string password;

    // 변수 선언
    public InputField idInputField;
    public InputField passwordInputField;

    public GameObject loginButton;
    public GameObject goSignUpButton;
    public GameObject signupButton;
    public GameObject cteatetext;
    public TextMeshProUGUI messageText;

    public WebSocket webSocket;
    public bool existingUser = false;
    public bool signupSuccess = false;
    public void Start()
    {
        webSocket = new WebSocket("wss://port-0-docker-node-be-1ru12mlvsjr55i.sel5.cloudtype.app/:3000");
        webSocket.OnMessage += OnMessage;
        webSocket.OnClose += OnClose;
        webSocket.Connect();
    }
    public void goSignUp()
    {
        loginButton.SetActive(false);
        goSignUpButton.SetActive(false);
        signupButton.SetActive(true);
        cteatetext.SetActive(true);
        // 입력 필드 초기화
        GameObject.Find("LoginCanvas/InputFieldID").GetComponent<InputField>().text = "";
        GameObject.Find("LoginCanvas/InputFieldPS").GetComponent<InputField>().text = "";

    }
    public void SignUp()
    {
        nickname = GameObject.Find("LoginCanvas/InputFieldID").GetComponent<InputField>().text;
        password = GameObject.Find("LoginCanvas/InputFieldPS").GetComponent<InputField>().text;
        Debug.Log("enter signup");
        int progress_st = 1;
        webSocket.Send($"{{\"progress_st\": \"{progress_st}\",\"nickname\": \"{nickname}\", \"password\": \"{password}\"}}");
        Debug.Log("send data");
        Debug.Log(webSocket);

        StartCoroutine(WaitForSignupResult(nickname));
    }
    private IEnumerator WaitForSignupResult(string nickname)
    {
        Debug.Log("message");
        // 메시지를 받을 때까지 대기
        while (!signupSuccess)
        {
            yield return null;
        }
        Debug.Log("message receive");
        // 회원가입 성공 시
        if (signupSuccess)
        {
            Debug.Log("회원가입 성공! 사용자 이름: " + nickname);
            messageText.text = "회원가입 성공!";
            StartCoroutine(ClearMessageText(2f));
            // UI 업데이트
            signupButton.SetActive(false);
            cteatetext.SetActive(false);
            loginButton.SetActive(true);
            goSignUpButton.SetActive(true);
        }
        // 이미 등록된 사용자인 경우
        else
        {
            Debug.Log("이미 등록된 사용자입니다.");
            messageText.text = "이미 등록된 사용자입니다.";
            StartCoroutine(ClearMessageText(2f));
        }
        GameObject.Find("LoginCanvas/InputFieldID").GetComponent<InputField>().text = "";
        GameObject.Find("LoginCanvas/InputFieldPS").GetComponent<InputField>().text = "";
    }
    public void OnMessage(object sender, MessageEventArgs e)
    {
        // 받은 메시지를 확인하여 처리
        int response = int.Parse(e.Data);
        if (response == 1)
        {
            Debug.Log("로그인 성공");
            signupSuccess = true;
        }
        else if (response == 2)
        {
            Debug.Log("로그인 실패");
            signupSuccess = false;
        }
    }
    public void OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket connection closed.");
    }
    private void OnApplicationQuit()
    {
        // 애플리케이션 종료 시 WebSocket 연결 종료
        if (webSocket != null && webSocket.IsAlive)
        {
            webSocket.Close();
        }
    }
    IEnumerator ClearMessageText(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageText.text = "";
    }
}
