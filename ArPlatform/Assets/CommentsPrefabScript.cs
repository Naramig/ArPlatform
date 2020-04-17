using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentsPrefabScript : MonoBehaviour
{
    public string name;
    public string message;
    public Text nameText;
    public Text messageText;
    // Start is called before the first frame update
    void Start()
    {
        nameText.text = name;
        messageText.text = message;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
