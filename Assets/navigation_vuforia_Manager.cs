using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class navigation_vuforia_Manager : MonoBehaviour
{
    public GameObject minimap; // 미니맵 GameObject
    public GameObject idpanel; // ID 패널 GameObject
    public GameObject searchinput; // 검색 입력 필드 GameObject
    public GameObject searchbutton; // 검색 버튼 GameObject
    public GameObject firsttext;

    public CloudRecoBehaviour mCloudRecoBehaviour; // CloudRecoBehaviour 컴포넌트
    public int st1;
    bool mIsScanning = false;
    string mTargetMetadata = "";
    private void Start()
    {
        st1 = PlayerPrefs.GetInt("ST");
    }
    void Awake()
    {
        if (st1 == 2)
        {
            mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
            mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
            mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
            mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
            mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
            mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
            Debug.Log("Awake");
        }
    }

    void OnDestroy()
    {
        if (mCloudRecoBehaviour != null)
        {
            mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
            mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
            mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
            mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
            mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
        }
    }

    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult)
    {
        // 이미지 타겟이 감지되었을 때 호출됩니다.
        if (cloudRecoSearchResult != null)
        {
            mTargetMetadata = cloudRecoSearchResult.MetaData;

            Debug.Log("Detected Image Target: " + cloudRecoSearchResult.TargetName);

            mCloudRecoBehaviour.enabled = false;
            Debug.Log("image found");
            firsttext.SetActive(false);
            StartNavigation();
        }
    }
    public void OnInitialized(CloudRecoBehaviour cloudRecoBehaviour)
    {
        Debug.Log("Cloud Reco initialized");
    }

    public void OnInitError(CloudRecoBehaviour.InitError initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    public void OnUpdateError(CloudRecoBehaviour.QueryError updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;

        if (scanning)
        {
            // Clear all known targets
        }
    }
    void StartNavigation()
    {
        // 이미지 타겟이 감지되면 실행할 동작을 정의합니다.
        // 여기에는 원하는 동작을 추가하시면 됩니다.
        Debug.Log("Image target detected! Starting navigation...");

        minimap.SetActive(true);
        idpanel.SetActive(true);
        searchinput.SetActive(true);
        searchbutton.SetActive(true);
            
    }
}
