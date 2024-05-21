using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SimpleCloudRecoEventHandler : MonoBehaviour
{
    CloudRecoBehaviour mCloudRecoBehaviour;
    bool mIsScanning = false;
    string mTargetMetadata = "";
    public GameObject firsttext;
    public ImageTargetBehaviour ImageTargetTemplate;
    public Navigation_Manager NM;
    public int st1;
    public string targetName;

    // Register cloud reco callbacks
    void Awake()
    {
        st1 = PlayerPrefs.GetInt("ST");
        if(st1==2)
        {
            mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
            mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
            mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
            mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
            mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
            mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
        }
    }
    //Unregister cloud reco callbacks when the handler is destroyed
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

    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult)
    {
        // Store the target metadata
        mTargetMetadata = cloudRecoSearchResult.MetaData;
        targetName = cloudRecoSearchResult.TargetName;
        Debug.Log(targetName);

        // Stop the scanning by disabling the behaviour
        mCloudRecoBehaviour.enabled = false;
        firsttext.SetActive(false);
        NM.start_positioin_set();
    }

    void OnGUI()
    {
        st1 = PlayerPrefs.GetInt("ST");
        if(st1==2)
        {
            // Display current 'scanning' status
            GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
            // Display metadata of latest detected cloud-target
            GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);
            // If not scanning, show button
            // so that user can restart cloud scanning
            if (!mIsScanning)
            {
                if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning"))
                {
                    // Reset Behaviour
                    mCloudRecoBehaviour.enabled = true;
                    mTargetMetadata = "";
                }
            }
        }
    }
}