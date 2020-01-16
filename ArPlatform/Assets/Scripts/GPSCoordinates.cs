using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSCoordinates : MonoBehaviour
{
    public Text gps;
    public Text dist;
    public Text debugText;
    private GameObject instance;
    public GameObject ufo;

    public Button setObjectHere;
    
    private int distance;
    private double LatOfObject = 42.885216;
    private double LongOfObject = 74.568994;
    private double LatLocal = 0.0;
    private double LongLocal = 0.0;  
    private bool exist = true;
    IEnumerator Start()
    {
        Button btn = setObjectHere.GetComponent<Button>();
        btn.onClick.AddListener(setCoordinates);
        debugText.text = LatOfObject.ToString() + " " + LongOfObject.ToString();
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
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
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            LatLocal = Input.location.lastData.latitude;
            LongLocal = Input.location.lastData.longitude;
            gps.text = "lat = " + Input.location.lastData.latitude + " \nlong = " + Input.location.lastData.longitude;
        }

    }
    void setCoordinates() {
        LatOfObject = LatLocal;
        LongOfObject = LongLocal;
        debugText.text = "Lat=" + LatOfObject.ToString() + "\n Long=" + LongOfObject.ToString();
    }
    private void FixedUpdate()
    {
        Input.location.Start();

        //examples
        // print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

        gps.text = "lat = " + Input.location.lastData.latitude + " long = " + Input.location.lastData.longitude;
        distance = Convert.ToInt32(Calculatedistance(Input.location.lastData.latitude, Input.location.lastData.longitude, LatOfObject, LongOfObject) * 1000);
        LatLocal = Input.location.lastData.latitude;
        LongLocal = Input.location.lastData.longitude;
        dist.text = distance.ToString() + "m";

        if (distance < 20 && exist)
        {
            exist = false;
            Vector3 temp = new Vector3(0, 0, 3);
            instance = Instantiate(ufo, temp, Quaternion.identity);
            instance.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;
        }
        else if(distance > 20 && !exist) {
            exist = true;
            Destroy(instance);
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