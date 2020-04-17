using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CommentsScript : MonoBehaviour
{
    public InputField commentText;
    string addComment = ServerInfo.ServerPath + "/setMessage";
    string getMessages = ServerInfo.ServerPath + "/getMessages/";
    public GameObject objectItem;
    public GameObject canvas;
    public GameObject commentsPanel;
    void Start()
    {
        getNewComments();
    }
    public void HideCommentsPanel() {
        commentsPanel.SetActive(false);
    }
    void getNewComments()
    {
        string groupId = SaveObjectInfoToFind.Name;
        WWW ww = new WWW(getMessages + groupId);
        StartCoroutine(WaitForRequest(ww));
    }
    public void SendButtonListener()
    {
        UserInfo data = SaveUserData.LoadUserInfo();
        Comment n = new Comment()
        {
            objects_name = SaveObjectInfoToFind.Name,
            user_login = data.login,
            message = commentText.text
        };
        commentText.text = "";
        Debug.Log(n.message);
        StartCoroutine(setMessage(addComment, n));


        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Message");
        foreach (GameObject enemy in enemies)
            GameObject.Destroy(enemy);

        getNewComments();

    }
    // Update is called once per frame
    public IEnumerator setMessage(string url, Comment n)
    {
        var jsonData = JsonUtility.ToJson(n);
        Debug.Log(jsonData);

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


                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }

    }

    public class Comment
    {
        public string objects_name;
        public string user_login;
        public string message;
    }
    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Result!: " + www.text);// contains all the data sent from the server
            string json = www.text;
            RootObject myObject = JsonUtility.FromJson<RootObject>("{\"groups\":" + www.text + "}");
            Debug.Log(myObject.groups[0].name);
            if (myObject.groups.Length == 0)
            {
                Debug.Log("Not found");
            }
            else
            {
                int y = myObject.groups.Length * 100 / 2;
                for (int i = 0; i < myObject.groups.Length; i++)
                {
                    Debug.Log(myObject.groups[i].name);
                    GameObject temp = Instantiate(objectItem, new Vector3(0, 0, 0), Quaternion.identity);
                    temp.tag = "Message";
                    temp.transform.parent = canvas.transform;
                    temp.transform.position = new Vector3(0, y , 0);
                    temp.SetActive(true);
                    Debug.Log(myObject.groups[i].name);
                    temp.GetComponent<CommentsPrefabScript>().name = myObject.groups[i].name;
                    temp.GetComponent<CommentsPrefabScript>().message = myObject.groups[i].message;
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
        public string name;
        public string message;
    }


    [Serializable]
    public class RootObject
    {
        public Data[] groups;
    }
}