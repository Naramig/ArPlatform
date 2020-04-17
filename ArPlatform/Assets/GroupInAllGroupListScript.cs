using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GroupInAllGroupListScript : MonoBehaviour
{
    public int id;
    public string name;
    public string description;
    public bool availability;
    string attachedUserToGroupUrl = ServerInfo.ServerPath + "/attachedUserToGroupFromFind";

    public Text nameText;
    public InputField passwordInputField;
    void Start()
    {
        nameText.text = name;

        if (availability)
            passwordInputField.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddGroup() {
        UserInfo data = SaveUserData.LoadUserInfo();
        var userInfo = new Data()
        {
            group_id = id,
            user_login = data.login,
            password = passwordInputField.text
        };

        StartCoroutine(AttachedUser(attachedUserToGroupUrl, userInfo));
    }
    public IEnumerator AttachedUser(string url, Data group)
    {
        var jsonData = JsonUtility.ToJson(group);
        //____________________
        Debug.Log(jsonData);
        //____________________

        using (UnityWebRequest www = UnityWebRequest.Post(url, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    Debug.Log(result);
                    if (result != "error")
                    {
                        Debug.Log(result);
                        Destroy(this.gameObject);

                    }
                    else
                    {
                        Debug.Log("Error");
                    }

                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }
    }


    [Serializable]

public class Data
    {
        public int group_id;
        public string user_login;
        public string password;
    }
}
