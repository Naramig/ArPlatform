using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginSystem : MonoBehaviour
{
    public InputField loginInputField;
    public InputField passwordInputField;
    string postUrl = ServerInfo.ServerPath + "/checkUser";

    public string login;
    public string password;
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        //just for test
       // SaveUserData.Delete();
        //-------------


        //Check if user is alredy signed in
        UserInfo data = SaveUserData.LoadUserInfo();
        if (data != null)
        {
            SceneManager.LoadScene("MainScene");
        }

    }

    public void Registration()
    {
        SceneManager.LoadScene("Registration");
    }
    public void SignIn()
    {
        login = loginInputField.text;
        password = passwordInputField.text;
        var user = new userAuData()
        {
            login = this.login,
            password = this.password
        };
        StartCoroutine(Post(postUrl, user));

        

    }
    public IEnumerator Post(string url, userAuData user)
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
                    if (result != "error")
                    {
                        name = result.ToString();
                        SaveUserData.SaveUser(this);
                        SceneManager.LoadScene("MainScene");
                    }
                    else {
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
