using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMang : MonoBehaviour
{

    // Start is called before the first frame update
    
    public void Choose3dObject()
    {
        SceneManager.LoadScene("Choose");

    }
    public void Set3dObject()
    {
        SceneManager.LoadScene("Set3d");

    }
}
