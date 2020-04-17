using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectInfoScript : MonoBehaviour
{
    string objectInfoURL = ServerInfo.ServerPath + "/ObjectInfo/";
    RootObject myObject;
    public Text nameOfTheObject;
    public Text descriptionText;
    public Text ratingNumber;
    public Text objectType;
    public Text coordinates;
    public Text OwnerNameText;
    // Start is called before the first frame update
    void Start()
    {
        WWW ww = new WWW(objectInfoURL + SaveInfoToPut3D.IdOfTheObjectToFind);
        StartCoroutine(WaitForRequest(ww));
    }
    public void FindTheObject()
    {
        SceneManager.LoadScene("FindObj");
    }
    public void GoBack()
    {
        SceneManager.LoadScene("FindObjectList");
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
                nameOfTheObject.text = myObject.groups[0].name;
                SaveObjectInfoToFind.Name = myObject.groups[0].name;
                descriptionText.text = myObject.groups[0].description;
                SaveObjectInfoToFind.Description = myObject.groups[0].description;
                ratingNumber.text = myObject.groups[0].rating.ToString();
                SaveObjectInfoToFind.Rating = myObject.groups[0].rating;
                objectType.text = myObject.groups[0].type_name;
                SaveObjectInfoToFind.Type = myObject.groups[0].type_name;
                coordinates.text = "Coordinates:\nLatitude " + myObject.groups[0].lat + "\nLongtitude " + myObject.groups[0].longt;
                SaveObjectInfoToFind.Lat = myObject.groups[0].lat;
                SaveObjectInfoToFind.Longt = myObject.groups[0].longt;
                OwnerNameText.text = myObject.groups[0].user_name;
                SaveObjectInfoToFind.Owner = myObject.groups[0].user_name;
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
        public string name;
        public string type_name;
        public float lat;
        public float longt;
        public string user_name;
        public string description;
        public int rating;
    }
    [Serializable]
    public class RootObject
    {
        public Data[] groups;
    }
}
