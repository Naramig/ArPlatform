﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroupsManage : MonoBehaviour
{
    string GroupsList = ServerInfo.ServerPath + "/groupsList/";
    public GameObject group;
    RootObject myObject;
    public GameObject canvas;
    int y = 800;

    // Start is called before the first frame update
    void Start()
    {
        UserInfo data = SaveUserData.LoadUserInfo();
        WWW ww = new WWW(GroupsList + data.login);
        StartCoroutine(WaitForRequest(ww));
    }
    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Result!: " + www.text);// contains all the data sent from the server
            string json = www.text;
            myObject = JsonUtility.FromJson<RootObject>("{\"groups\":" + www.text + "}");
            Debug.Log(myObject.groups[0].name);
            if (myObject.groups.Length == 0) {
                Debug.Log("Not found");
            }
            else {
                for (int i = 0; i < myObject.groups.Length; i++)
                {
                    Debug.Log(myObject.groups[i].name);
                    GameObject temp = Instantiate(group, new Vector3(600, y, 0), Quaternion.identity);
                    temp.transform.parent = canvas.transform;
                    temp.SetActive(true);
                    Debug.Log(myObject.groups[i].name);
                    temp.GetComponent<GroupData>().name = myObject.groups[i].name;
                    temp.GetComponent<GroupData>().id = myObject.groups[i].id;
                    y -= 100;
                }
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewGroup()
    {
        SceneManager.LoadScene("CreateNewGroup");
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
        public Data[] groups;
    }
}
