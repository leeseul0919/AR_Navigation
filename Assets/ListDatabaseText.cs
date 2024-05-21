using UnityEngine;
using UnityEngine.UI;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using TMPro;

public class ListDatabaseText : MonoBehaviour
{
    public InputField searchInputField;
    public GameObject resultPrefab;
    public Transform contentTransform;

    public List<string> des_name_list = new List<string>();

    private void Start()
    {
        searchInputField.onValueChanged.AddListener(delegate { OnSearchInputValueChanged(); });
    }

    // 입력 필드 값이 변경될 때마다 호출되는 이벤트 핸들러
    private void OnSearchInputValueChanged()
    {
        string searchText = searchInputField.text;

        // 검색어가 비어 있는 경우 모든 데이터를 불러오지 않도록 수정
        if (string.IsNullOrEmpty(searchText))
        {
            ClearContent();
        }
        else
        {
            SearchDatabaseList(searchText);
        }
    }

    private void ClearContent()
    {
        // Clear content
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SearchDatabaseList(string searchText)
    {
        // Clear previous results
        ClearContent();

        List<string> result = new List<string>();
        foreach(string item in des_name_list)
        {
            if (item.Contains(searchText)) result.Add(item);
        }

        if (result.Count > 0)
        {
            foreach (string document in result)
            {
                // Instantiate하기 전에 파괴되었는지 확인
                if (contentTransform == null) return;

                GameObject resultItem = Instantiate(resultPrefab, contentTransform);

                // resultItem이 null이 아닌지 확인
                if (resultItem != null)
                {
                    // TextMeshPro 텍스트 설정
                    TextMeshProUGUI textMeshPro = resultItem.GetComponentInChildren<TextMeshProUGUI>();
                    if (textMeshPro != null)
                    {
                        textMeshPro.text = document;
                    }
                    else
                    {
                        Debug.LogError("TextMeshPro component not found in the resultPrefab.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to instantiate resultPrefab.");
                }
            }
        }
        else
        {
            Debug.Log("No results found.");
        }
    }
}
