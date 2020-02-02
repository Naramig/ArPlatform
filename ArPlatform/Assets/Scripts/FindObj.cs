using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FindObj : MonoBehaviour
{
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
    private float LatLocal = 0.0f;
    private float LongLocal = 0.0f;  
    private bool exist = true;
    bool canCheckGPS = true;
    void Start()
    {
        LatOfObject = ObjInfo.Lat;
        LongOfObject = ObjInfo.Longt;
        name.text = ObjInfo.Name;
        Type.text = ObjInfo.Obj;
        debugText.text = "LatOfObj = " + LatOfObject + "\n LongOfObj = " + LongOfObject;
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
        if (distance < 20 && exist)
        {
            exist = false;
            Vector3 temp = new Vector3(0, 0, distance);
            instance = Instantiate(ufo, temp, Quaternion.identity);
            instance.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;

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