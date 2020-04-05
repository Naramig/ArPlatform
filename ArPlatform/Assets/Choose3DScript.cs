using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Choose3DScript : MonoBehaviour
{
    string other3DURL = ServerInfo.ServerPath + "/other3DList";
    string own3DURL = ServerInfo.ServerPath + "/own3DList";
    RootObject myObject;
    public GameObject item3D;
    public GameObject canvas;
    int y = 800;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GpToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void Other3D()
    {
        WWW ww = new WWW(other3DURL);
        StartCoroutine(WaitForRequestOther3D(ww));
    }
    IEnumerator WaitForRequestOther3D(WWW www)
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
                    GameObject temp = Instantiate(item3D, new Vector3(600, y, 0), Quaternion.identity);
                    temp.transform.parent = canvas.transform;
                    temp.SetActive(true);
                    Debug.Log(myObject.groups[i].name);
                    temp.GetComponent<ObjectScript>().name = myObject.groups[i].name;
                    temp.GetComponent<ObjectScript>().id = myObject.groups[i].id;
                    y -= 100;
                }
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void Own3D()
    {
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
