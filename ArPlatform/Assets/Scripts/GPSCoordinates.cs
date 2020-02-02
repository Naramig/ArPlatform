﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GPSCoordinates : MonoBehaviour
{
    public GameObject exCube;
    public Text gps;
    public Text Info;
    public GameObject ufo;
    public Button setObjectHere;

    private GameObject instance;
    private int distance;
    private float LatLocal = 0.0f;
    private float LongLocal = 0.0f;
    private bool exist = false;

    float LatOfObject = 0.0f;
    float LongOfObject = 0.0f;
    bool canCheckGPS = true;
    string postUrl = "http://69.55.60.116:3000/data";
    Button btn;

    void Start()
    {
        btn = setObjectHere.GetComponent<Button>();
        btn.onClick.AddListener(setCoordinates);
    }

    public void goToMenu()
    {
        SceneManager.LoadScene("MainScene");

    }
    void setCoordinates() {
        LatOfObject = LatLocal;
        LongOfObject = LongLocal;
        Destroy(exCube);
        var enemy = new Obj()
        {
            name = "TEST",
            lat = LatOfObject,
            longt = LongOfObject,
            objName = "Cube"
        };
        btn.gameObject.SetActive(false);
        StartCoroutine(Post(postUrl, enemy));
        exist = true;
    }

    public IEnumerator Post(string url, Obj enemy)
    {
        var jsonData = JsonUtility.ToJson(enemy);
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
                    Info.text = result;


                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (canCheckGPS)
        {
            StartCoroutine(CheckGPSLocation());
            canCheckGPS = false;
        }
        if (distance < 20 && exist)
        {
            exist = false;
            Vector3 temp = new Vector3(0, 0, 3);
            instance = Instantiate(ufo, temp, Quaternion.identity);
            instance.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 3;

        }
        else if (distance > 20 && !exist)
        {
            exist = true;
            Destroy(instance);
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
            gps.text = "lat = " + Input.location.lastData.latitude + " long = " + Input.location.lastData.longitude;
            distance = Convert.ToInt32(Calculatedistance(Input.location.lastData.latitude, Input.location.lastData.longitude, LatOfObject, LongOfObject) * 1000);
            LatLocal = Input.location.lastData.latitude;
            LongLocal = Input.location.lastData.longitude;
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