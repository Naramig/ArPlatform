using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ListAllGroups : MonoBehaviour
{
    string GroupsList = ServerInfo.ServerPath + "/allGroupsList/";
    RootObject myObject;
    public  GameObject content;
    public GameObject group;
    int y = 0;
    void Start()
    {
        UserInfo data = SaveUserData.LoadUserInfo();

        WWW ww = new WWW(GroupsList + data.login);
        StartCoroutine(WaitForRequest(ww));
    }
    public void BackToOwnGroups()
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
            myObject = JsonUtility.FromJson<RootObject>("{\"groups\":" + www.text + "}");
            Debug.Log(myObject.groups[0].name);
            if (myObject.groups.Length == 0)
            {
                Debug.Log("Not found");
            }
            else
            {
                y = myObject.groups.Length * 100 / 2;
               // content.GetComponent(RectTransform).sizeDelta = new Vector2(1, 1);
                for (int i = 0; i < myObject.groups.Length; i++)
                {
                    Debug.Log(myObject.groups[i].name);
                    GameObject temp = Instantiate(group, new Vector3(0, 0, 0), Quaternion.identity);
                    temp.transform.parent = content.transform;
                    temp.transform.localPosition = new Vector3(0, y, 0);
                    temp.SetActive(true);
                    Debug.Log(myObject.groups[i].name);
                    temp.GetComponent<GroupInAllGroupListScript>().name = myObject.groups[i].name;
                    temp.GetComponent<GroupInAllGroupListScript>().id = myObject.groups[i].id;
                    temp.GetComponent<GroupInAllGroupListScript>().availability = myObject.groups[i].availability;
                    temp.GetComponent<GroupInAllGroupListScript>().description = myObject.groups[i].description;
                    Debug.Log(myObject.groups[i].id);
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

   

    [Serializable]

    public class Data
    {
        public int id;
        public string name;
        public bool availability;
        public string description;
    }


    [Serializable]
    public class RootObject
    {
        public Data[] groups;
    }
}
