using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Registration : MonoBehaviour
{
    public InputField nameInputField;
    public InputField passwordInputField;
    public InputField loginInputField;

    public string login;
    public string password;
    public string name;

    string postUrl = ServerInfo.ServerPath + "/registration";

    // Start is called before the first frame update
    void Start()
    {
    
    }

    public void Back()
    {
        SceneManager.LoadScene("Login");
    }
    public void Registr()
    {
        name = nameInputField.text;
        login = loginInputField.text;
        password = passwordInputField.text;
        var user = new UserRegistrationData()
        {
            name = name,
            login = login,
            password = password
        };
        StartCoroutine(Post(postUrl, user));
    }
    public IEnumerator Post(string url, UserRegistrationData user)
    {
        var jsonData = JsonUtility.ToJson(user);
        //____________________
        Debug.Log(jsonData);
        //____________________

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
                    if (result == "1")
                    {
                        SaveUserData.SaveUser(this);
                        SceneManager.LoadScene("Login");
                       
                    }
                    else
                    {
                        Debug.Log("Error");
                    }

                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }
    }
}
