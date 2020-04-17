using Dummiesman;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GPSCoordinates : MonoBehaviour
{
    public GameObject cancel;
    public GameObject InfoToDB;
    public GameObject AddToBD;
    public GameObject loadSign;
    public GameObject preview;
    public GameObject exCube;
    public Text gps;
    public Text Info;
    public GameObject ufo;
    public Button setObjectHere;

    private GameObject instance;
    GameObject loadedObj;
    private int distance;
    private float LatLocal = 0.0f;
    private float LongLocal = 0.0f;
    private bool exist = false;

    public float LatOfObject = 0.0f;
    public float LongOfObject = 0.0f;
    bool canCheckGPS = true;
    Button btn;
    bool inPreview = false;
    string getObject = ServerInfo.ServerPath + "/getObject/";
    void Start()
    {
        // StartCoroutine(LoadFromServer());
        StartCoroutine(StartDownload());
        Debug.Log(0);
        Info.text = "Group's name - " + SaveInfoToPut3D.NameOfGroup + " \n Object's name - " + SaveInfoToPut3D.NameOfObject;


        Debug.Log(SaveInfoToPut3D.NameOfGroup + " \\ " + SaveInfoToPut3D.NameOfObject);
        btn = setObjectHere.GetComponent<Button>();
        btn.onClick.AddListener(setCoordinates);
    }
    
    public void AddToDatabase()
    {
        InfoToDB.SetActive(true);
        AddToBD.SetActive(false);
        cancel.SetActive(false);

    }
    public void Cancel()
    {
        Vector3 tmp = new Vector3(0.01f, 0.01f, 0.01f);
        loadedObj.transform.parent = preview.transform;
        loadedObj.transform.localScale = tmp;
        inPreview = true;
        loadedObj.transform.localPosition = new Vector3(0, 0, 0);
        btn.gameObject.SetActive(true);
    }
    public void goToMenu()
    {
        SceneManager.LoadScene("MainScene");

    }
    void setCoordinates() {
        LatOfObject = LatLocal;
        LongOfObject = LongLocal;
        //Destroy(exCube);
        btn.gameObject.SetActive(false);
        exist = true;
    }
    IEnumerator StartDownload()
    {
        using (var www = new WWW(getObject + SaveInfoToPut3D.NameOfObject))
        {
            yield return www;
            AddToBD.SetActive(true);
            setObjectHere.gameObject.SetActive(true);
            var textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.text));
            loadedObj = new OBJLoader().Load(textStream);
             Vector3 tmp = new Vector3(0.01f, 0.01f, 0.01f);
            loadedObj.transform.parent = preview.transform;
            loadedObj.transform.localScale = tmp;
            ChangePreview();
            
        }
    }
    public void ChangePreview()
    {
        loadedObj.transform.localPosition = new Vector3(0, 0, 0);
        loadSign.SetActive(false);
        Debug.Log("Preview");
        inPreview = true;
    }
    
    private void FixedUpdate()
    {
        exCube.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
        if (canCheckGPS)
        {
            StartCoroutine(CheckGPSLocation());
            canCheckGPS = false;
        }
        if (distance < 20 && exist)
        {
            exist = false;
            /*Vector3 temp = new Vector3(0, 0, 3);
            instance = Instantiate(ufo, temp, Quaternion.identity);
            instance.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;*/
            inPreview = false;
            loadedObj.transform.parent = exCube.transform;
            loadedObj.transform.parent = null;


            exCube.SetActive(false);

            loadedObj.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
            loadedObj.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            loadedObj.transform.rotation = transform.rotation;
        }
        else if (distance > 20 && !exist)
        {
            exist = true;
            Destroy(instance);
        }

        //Preview of 3D object
        if (inPreview) {
            loadedObj.transform.Rotate(0, -30 * Time.deltaTime, 0);
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