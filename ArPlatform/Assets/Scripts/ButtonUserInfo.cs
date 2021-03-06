﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonUserInfo : MonoBehaviour
{
    public GameObject btn;
    public string name;
    public bool notFound = true;
    public int id;
    public string login;
    public int group_id;
  public  bool newUser = true;
    string attachedUserToGroupUrl = ServerInfo.ServerPath + "/attachedUserToGroup";
    CreatingGroup group;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void adduserToTheGroup()
    {
        

        var userData = new UserGroupInfo()
        {
            login = this.login,
            admin = false,
            group_id = group_id
        };

        StartCoroutine(MakeUserAdmin(attachedUserToGroupUrl, userData));
    }
    public IEnumerator MakeUserAdmin(string url, UserGroupInfo adminInfo)
    {
        var jsonData = JsonUtility.ToJson(adminInfo);
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


                        btn.GetComponentInChildren<Text>().text = "Added";
                        btn.gameObject.SetActive(false);

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
    void Update()
    {
        if (newUser)
        {
            newUser = false;
            if (notFound)
            {
                btn.gameObject.SetActive(false);
                this.GetComponentInChildren<Text>().text = "Not Found";
            }
            else
            {
                btn.gameObject.SetActive(true);
                this.GetComponentInChildren<Text>().text = name.ToString();
                btn.GetComponentInChildren<Text>().text = "Add";

            }
        } 
        
    }

    [System.Serializable]

    public class UserGroupInfo
    {
        public string login;
        public int group_id;
        public bool admin;
    }
}
