using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetObjectToWorld : MonoBehaviour
{
    public InputField nameOfObject;
    public InputField descriptionOfObject;
    public GameObject Coordinates;
    string postUrl = ServerInfo.ServerPath + "/setObjectToWorld";
    void Start()
    {
        
    }
    public void Set()
    {
        UserInfo data = SaveUserData.LoadUserInfo();
        ObjectInfo n = new ObjectInfo()
        {
            name = nameOfObject.text,
            objectTypeName = SaveInfoToPut3D.NameOfObject,
            userGroupName = SaveInfoToPut3D.NameOfGroup,
            lat = Coordinates.GetComponent<GPSCoordinates>().LatOfObject,
            longt = Coordinates.GetComponent<GPSCoordinates>().LongOfObject,
            userLogin = data.login,
            description = descriptionOfObject.text
        };
        Debug.Log(n.name);
        StartCoroutine(Post(postUrl, n));
        this.gameObject.SetActive(false);
    }
    public IEnumerator Post(string url, ObjectInfo n)
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
    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]

    public class ObjectInfo
    {
        public string name;
        public string objectTypeName;
        public float lat;
        public float longt;
        public string userLogin;
        public string userGroupName;
        public string description;
    }
}
