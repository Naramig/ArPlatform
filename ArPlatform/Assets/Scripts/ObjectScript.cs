﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    public string name = "";
    public int id;
    void Start()
    {
        text.text = name;
    }

    // Update is called once per frame
    void Update()
    {
       // text.text = name;
    }

    public void OkButtonClick()
    {
        SaveInfoToPut3D.NameOfObject = name;
        SceneManager.LoadScene("Set3d");
    }
}
