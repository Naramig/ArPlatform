using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArObj : MonoBehaviour
{
    public string name;
    public float lat;
    public float longt;
    public string obj;
    public int id;

    private void Start()
    {
        this.GetComponentInChildren<Text>().text = id.ToString();
    }

    public void setOnObjInfo()
    {
        ObjInfo.Name = name;
        ObjInfo.Longt = longt;
        ObjInfo.Id = id;
        ObjInfo.Obj = obj;
        ObjInfo.Lat = lat;
        SceneManager.LoadScene("FindObj");

    }
}
