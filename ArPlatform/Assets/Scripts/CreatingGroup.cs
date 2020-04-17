using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreatingGroup : MonoBehaviour
{
    string userLogin = "";
    public GameObject listOfUsers;
    public GameObject addUserPanel;
    // Start is called before the first frame update
    public InputField nameInputFieald;
    public InputField passwordInputField;
    public InputField descriptionInputField;
    public InputField userLoginInputField;
    public GameObject registrationPanel;
    public int group_id = 0;
    bool availability = true;
    int nextUserPage = 0;
    RootObject myObject;
    float y = -200;
    float x = 200;

    string createGroupUrl = ServerInfo.ServerPath + "/createGroup";
    string attachedUserToGroupUrl = ServerInfo.ServerPath + "/attachedUserToGroup";
    string getAllUsers = ServerInfo.ServerPath + "/getUsers/";

    void Start()
    {


    }

    public void CreateGroup()
    {
        if (passwordInputField.text.Length != 0)
        {
            availability = false;
        }
        Debug.Log(availability + "________________");
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
                        registrationPanel.SetActive(false);
                        addUserPanel.SetActive(true);
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
                      //  userLoginInputField.setVisible = true;


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

    public void Search()
    {
        userLogin = userLoginInputField.text;
        WWW ww = new WWW(getAllUsers + userLogin);
        StartCoroutine(WaitForRequest(ww)); 
    }

    public void goToGroups()
    {
        SceneManager.LoadScene("Groups");
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
            if (myObject.users.Length == 0) {
                Debug.Log("Not found");
                listOfUsers.GetComponent<ButtonUserInfo>().notFound = true;
                listOfUsers.GetComponent<ButtonUserInfo>().newUser = true;

            }
            else {
                listOfUsers.SetActive(true);
                listOfUsers.GetComponent<ButtonUserInfo>().notFound = false;
                listOfUsers.GetComponent<ButtonUserInfo>().name = myObject.users[0].name;
                listOfUsers.GetComponent<ButtonUserInfo>().login = userLogin;
                listOfUsers.GetComponent<ButtonUserInfo>().id = myObject.users[0].id;
                listOfUsers.GetComponent<ButtonUserInfo>().group_id = group_id;
                listOfUsers.GetComponent<ButtonUserInfo>().newUser = true;
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
        public string name;
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
