using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FindObjectData : MonoBehaviour
{
    public int id;
    public string name;
    public Text nameText;
    // Start is called before the first frame update
    void Start()
    {
        nameText.text = name;
    }

    public void InfoAboutObject()
    {
        SaveInfoToPut3D.IdOfTheObjectToFind = id;
        SceneManager.LoadScene("InfoAboutObject");
    }
}
