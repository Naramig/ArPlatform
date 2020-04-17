using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ListOfFindObject : MonoBehaviour
{
    string ObjectList = ServerInfo.ServerPath + "/objectList/";
    public GameObject objectItem;
    RootObject myObject;
    public GameObject canvas;
    int y = 800;

    // Start is called before the first frame update
    void Start()
    {
        int groupId = SaveGroupInfo.GroupId;
        WWW ww = new WWW(ObjectList + groupId);
        StartCoroutine(WaitForRequest(ww));
    }
    public void GoBack() {
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
                for (int i = 0; i < myObject.groups.Length; i++)
                {
                    Debug.Log(myObject.groups[i].name);
                    GameObject temp = Instantiate(objectItem, new Vector3(600, y, 0), Quaternion.identity);
                    temp.transform.parent = canvas.transform;
                    temp.SetActive(true);
                    Debug.Log(myObject.groups[i].name);
                    temp.GetComponent<FindObjectData>().name = myObject.groups[i].name;
                    temp.GetComponent<FindObjectData>().id = myObject.groups[i].id;
                    y -= 100;
                }
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
        public Data[] groups;
    }
}