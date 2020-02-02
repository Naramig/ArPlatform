using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GetDataFromServer : MonoBehaviour
{
    RootObject myObject;
    public GameObject canvas;
    public Button btn;
    float y = 1900;
    float x = 200;
    void Start()
    {
        string url = "http://69.55.60.116:3000/getAll";
        // RootObject = new Data();

        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
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
            Debug.Log(myObject.users[1].name);
            for (int i = 0; i < myObject.users.Length; i++)
            {
                Button temp = Instantiate(btn);
                temp.GetComponent<ArObj>().name = myObject.users[i].name;
                temp.GetComponent<ArObj>().lat = myObject.users[i].lat;
                temp.GetComponent<ArObj>().longt = myObject.users[i].longt;
                temp.GetComponent<ArObj>().obj = myObject.users[i].obj;
                temp.GetComponent<ArObj>().id = myObject.users[i].id;

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
    public string name;
    public float lat;
    public float longt;
    public string obj;
}


[Serializable]
public class RootObject
{
    public Data[] users;
}
}