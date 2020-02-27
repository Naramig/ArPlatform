using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreatingGroup : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField nameInputFieald;
    public InputField passwordInputField;
    public InputField descriptionInputField;
    public int group_id = 0;
    bool availability = false;

    public GameObject userButton;
    public GameObject canvas;
    RootObject myObject;
    float y = 0;
    float x = 200;

    string createGroupUrl = ServerInfo.ServerPath + "/createGroup";
    string attachedUserToGroupUrl = ServerInfo.ServerPath + "/attachedUserToGroup";
    string getAllUsers = ServerInfo.ServerPath + "/getUsers/";

    void Start()
    {


    }

    public void CreateGroup()
    {
        if (passwordInputField.text != "")
        {
            availability = false;
        }
        var group = new GroupInfo()
        {
            name = nameInputFieald.text,
            password = passwordInputField.text,
            description = descriptionInputField.text,
            availability = this.availability
        };

        StartCoroutine(Post(createGroupUrl, group));
    }

    public IEnumerator Post(string url, GroupInfo group)
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
                        group_id = Int32.Parse(result);
                        UserInfo data = SaveUserData.LoadUserInfo();

                        var adminInfo = new UserGroupInfo()
                        {
                            login = data.login,
                            group_id = Int32.Parse(result),
                            admin = true
                         };

                        StartCoroutine(MakeUserAdmin(attachedUserToGroupUrl, adminInfo));

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

    public IEnumerator MakeUserAdmin(string url, UserGroupInfo userGroupInfo)
    {
        var jsonData = JsonUtility.ToJson(userGroupInfo);
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

                        WWW ww = new WWW(getAllUsers + "0");
                        StartCoroutine(WaitForRequest(ww));                    
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
    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Result!: " + www.text);// contains all the data sent from the server
            string json = www.text;
            myObject = JsonUtility.FromJson<RootObject>("{\"users\":" + www.text + "}");
            Debug.Log(myObject.users[1].login);
            for (int i = 0; i < myObject.users.Length; i++)
            {
                GameObject temp = Instantiate(userButton);
                temp.GetComponent<ButtonUserInfo>().login = myObject.users[i].login;
                temp.GetComponent<ButtonUserInfo>().id = myObject.users[i].id;
                temp.GetComponent<ButtonUserInfo>().group_id = group_id;

                temp.transform.SetParent(canvas.transform);
                temp.transform.position = new Vector3(x, y, 0);
                y -= 100;
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }



    [Serializable]

    public class Data
    {
        public int id;
        public string login;
    }


    [Serializable]
    public class RootObject
    {
        public Data[] users;
    }

    [System.Serializable]

    public class GroupInfo
    {
        public string name;
        public string description;
        public string password;
        public bool availability;
    }

    [System.Serializable]

    public class UserGroupInfo
    {
        public string login;
        public int group_id;
        public bool admin;
    }
}
