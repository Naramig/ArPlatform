using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RightBarFindObjScript : MonoBehaviour
{
    public GameObject commentPanel;
    public GameObject LikeButtonAnimation;
    public GameObject DislikeButtonAnimation;
    public Text rating;
    public Button LikeButton;
    public Button DislikeButton;
    int like = 0;
    string setRatingUrl = ServerInfo.ServerPath + "/setRating";
    private Animator anim;
    private void Start()
    {
        rating.text = SaveObjectInfoToFind.Rating.ToString();
    }
    public void OpenCommentsPanel()
    {
        commentPanel.SetActive(true);
    }
    public void LikeButtonListener()
    {
        like = 1;
        rating.text = (SaveObjectInfoToFind.Rating + 1).ToString();
        StartAnimationOnScreen(LikeButtonAnimation);
    }
    public void DislikeButtonListener()
    {
        like = -1;
        rating.text = (SaveObjectInfoToFind.Rating - 1).ToString();
        StartAnimationOnScreen(DislikeButtonAnimation);

    }
    public  void StartAnimationOnScreen(GameObject gm)
    {
        RatingInfo n = new RatingInfo()
        {
            rating = like,
            object_name = SaveObjectInfoToFind.Name
    };
        Debug.Log(n.rating);
        StartCoroutine(Post(setRatingUrl, n));

        gm.SetActive(true);
        anim = gm.gameObject.GetComponent<Animator>();
        anim.Play("LikeAnimation", 0);
        Destroy(LikeButtonAnimation, 1.2f);
        Destroy(DislikeButtonAnimation, 1.2f);
        LikeButton.interactable = false;
        DislikeButton.interactable = false;

    }
    public IEnumerator Post(string url, RatingInfo n)
    {
        var jsonData = JsonUtility.ToJson(n);
        Debug.Log(jsonData);

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


                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }

    }
    [System.Serializable]

    public class RatingInfo
    {
        public int rating;
        public string object_name;
        
    }
}
