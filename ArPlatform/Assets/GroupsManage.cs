using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroupsManage : MonoBehaviour
{
    string GroupsList = ServerInfo.ServerPath + "/groupsList";
    public GameObject group;
    RootObject myObject;

    // Start is called before the first frame update
    void Start()
    {
        UserInfo data = SaveUserData.LoadUserInfo();
        WWW ww = new WWW(GroupsList + data.name);
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
            myObject = JsonUtility.FromJson<RootObject>("{\"users\":" + www.text + "}");
            if (myObject.groups.Length == 0) {
                Debug.Log("Not found");
            }
            else {
                for (int i = 0; i < myObject.groups.Length; i++)
                {
                    Debug.Log(myObject.groups[i].name);
                    group.SetActive(true);
                    group.GetComponent<GroupData>().name = myObject.groups[i].name;
                    group.GetComponent<GroupData>().id = myObject.groups[i].id;
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
