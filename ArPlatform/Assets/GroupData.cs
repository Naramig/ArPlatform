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
    // Start is called before the first frame update
    void Start()
    {
        nameOfTheGroup.text = name;
    }
    private void FixedUpdate()
    {
        nameOfTheGroup.text = name;
    }
    public void ChooseGroup()
    {
        SaveGroupInfo.GroupId = id;
        SceneManager.LoadScene("Objects");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
