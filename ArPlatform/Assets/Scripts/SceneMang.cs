using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMang : MonoBehaviour
{
    public Text userName;

    private void Start()
    {
        UserInfo data = SaveUserData.LoadUserInfo();
        userName.text = data.name;
    }
    public void Choose3dObject()
    {
        SceneManager.LoadScene("Choose");

    }
    public void Set3dObject()
    {
        SceneManager.LoadScene("Set3d");

    }
    public void Groups()
    {
        SceneManager.LoadScene("Groups");
    }

    public void LogOut()
    {
        SaveUserData.Delete();
        SceneManager.LoadScene("Login");
    }
}
