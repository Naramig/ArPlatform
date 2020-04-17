using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GroupData : MonoBehaviour
{
    public Text nameOfTheGroup;
    public string name;
    public int id;
    public bool isAdmin = false;
    public GameObject chooseObj;
    // Start is called before the first frame update
    void Start()
    {
        nameOfTheGroup.text = name;
        if (!isAdmin)
        {
            chooseObj.SetActive(false);
        }
    }
   
    public void SetObject()
    {
        SaveInfoToPut3D.NameOfGroup = name;
        SaveGroupInfo.GroupId = id;
         

        SceneManager.LoadScene("Choose3d");
    }
    public void FindObject()
    {
        SaveGroupInfo.GroupId = id;
        SceneManager.LoadScene("FindObjectList");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OkButtonListener()
    {
        SaveInfoToPut3D.NameOfGroup = name;
        SceneManager.LoadScene("Choose3D");
    }
}
