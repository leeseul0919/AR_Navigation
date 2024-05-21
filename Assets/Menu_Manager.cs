using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour
{
    public Text menu_text;
    public Text menu_state;
    public GameObject obstacle_function;
    public string nickname;
    public int st;
    public int manager_check;
    void Start()
    {
        nickname = PlayerPrefs.GetString("Nick");
        manager_check = PlayerPrefs.GetInt("ManagerCheck");
        obstacle_function = GameObject.Find("Canvas/Button");
        menu_text.text = "Hi " + nickname;
        if (manager_check != 1)
        {
            obstacle_function.SetActive(false);
            menu_state.text = "This is User Session.";
        }
        else
        {
            menu_state.text = "This is Manager Session.";
        }
    }
    public void goto_obstaclemanage()
    {
        st = 1;
        PlayerPrefs.SetInt("ST", st);
        SceneManager.LoadScene("SampleScene");
    }
    // Update is called once per frame
    public void goto_navigation()
    {
        st = 2;
        PlayerPrefs.SetInt("ST", st);
        SceneManager.LoadScene("SampleScene");
    }
}
