using Dummiesman;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FindObj : MonoBehaviour
{

    public GameObject rightPanel;
    public GameObject loadingUi;
    public GameObject finishDownload;


    public Text name;
    public Text Type;
    public Text gps;
    public Text dist;
    public Text debugText;
    private GameObject instance;
    public GameObject ufo;   
    private int distance;
    float LatOfObject = 42.885216f;
    float LongOfObject = 74.568994f;
    public float LatLocal = 0.0f;
    public float LongLocal = 0.0f;  
    private bool exist = true;
    bool canCheckGPS = true;
    string getObject = ServerInfo.ServerPath + "/getObject/";
    GameObject loadedObj;
    bool isLoaded = false;
    void Start()
    {
        StartCoroutine(StartDownload());
        LatOfObject = SaveObjectInfoToFind.Lat;
        LongOfObject = SaveObjectInfoToFind.Longt;
        name.text = SaveObjectInfoToFind.Name;
        Type.text = SaveObjectInfoToFind.Type;
        debugText.text = "LatOfObj = " + LatOfObject + "\n LongOfObj = " + LongOfObject;
    }
    IEnumerator StartDownload()
    {
        using (var www = new WWW(getObject + SaveObjectInfoToFind.Type))
        {
            yield return www;
            loadingUi.SetActive(false);
            finishDownload.SetActive(true);
            var textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.text));
            loadedObj = new OBJLoader().Load(textStream);
            loadedObj.SetActive(false); ;
            isLoaded = true;
        }
    }
    public void goToMenu()
    {
        SceneManager.LoadScene("MainScene");

    }


    private void FixedUpdate()
    {
        if (canCheckGPS)
        {
            StartCoroutine(CheckGPSLocation());
            canCheckGPS = false;
        }
        if (distance < 20 && exist && isLoaded)
        {
            rightPanel.SetActive(true);
            exist = false;
            loadedObj.SetActive(true); ;
            loadedObj.transform.rotation = Quaternion.identity;
            loadedObj.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
            loadedObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        }
        else if (distance > 20 && !exist && isLoaded)
        {
            rightPanel.SetActive(false);
            exist = true;
            loadedObj.SetActive(false); 
        }


    }
    IEnumerator CheckGPSLocation()
    {
        yield return new WaitForSeconds(1.0f);
        int maxWait = 20;
        canCheckGPS = true;
        Input.location.Start();
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            //    print("Unable to determine device location");
            yield break;
        }
        else
        {
             LatLocal = Input.location.lastData.latitude;
             LongLocal = Input.location.lastData.longitude;
            gps.text = "lat = " + LatLocal + " long = " + LongLocal;
            distance = Convert.ToInt32(Calculatedistance(LatLocal, LongLocal, LatOfObject, LongOfObject) * 1000);
          
            dist.text = distance.ToString() + "m";
        }
       

        

    }
    private double Calculatedistance(double lat1, double lon1, double lat2, double lon2)
    {
        if ((lat1 == lat2) && (lon1 == lon2))
        {
            return 0;
        }
        else
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            dist = dist * 1.609344;     //convert to km.
            return (dist);
        }
    }


    //  This function converts decimal degrees to radians             
    private double deg2rad(double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    // This function converts radians to decimal degrees           
    private double rad2deg(double rad)
    {
        return (rad / Math.PI * 180.0);
    }
}