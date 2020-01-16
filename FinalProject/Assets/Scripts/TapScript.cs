using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TapScript : MonoBehaviour
{
    public Button btn1;
    public Button btn2;
    public Button btn3;

    public Image img1;
    public Image img2;
    public Image img3;


    public Button menu;
    public Button pause;
    bool isPause = false;
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public int price = 5;
    int whichWeapon = 0;
    public int coins = 10;
    public Text coinsN;
    
    public AudioSource coinAudio;
    public AudioSource Music;


    private void Start()
    {

        Music.Play( 0);

        menu.gameObject.SetActive(false);

        img1.gameObject.SetActive(false);
        img2.gameObject.SetActive(false);
        img3.gameObject.SetActive(false);

        btn1.onClick.AddListener(UseWeapon1);
        btn2.onClick.AddListener(UseWeapon2);
        btn3.onClick.AddListener(UseWeapon3);
        pause.onClick.AddListener(Pause);
        menu.onClick.AddListener(GoToMenu);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;

    }

    void Pause() {
        if (!isPause)
        {
            Time.timeScale = 0;
            menu.gameObject.SetActive(true);
            isPause = true;
        }
        else
        {
            Time.timeScale = 1;
            menu.gameObject.SetActive(false);
            isPause = false;
        }
        }
    void UseWeapon1()
    {
        print("1");
        whichWeapon = 1;
        img1.gameObject.SetActive(true);
        img2.gameObject.SetActive(false);
        img3.gameObject.SetActive(false);

    }
    void UseWeapon2()
    {
        img1.gameObject.SetActive(false);
        img2.gameObject.SetActive(true);
        img3.gameObject.SetActive(false);
        whichWeapon = 2;

        print("2");
        
    }
    void UseWeapon3()
    {
        img1.gameObject.SetActive(false);
        img2.gameObject.SetActive(false);
        img3.gameObject.SetActive(true);
        whichWeapon = 3;
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Convert mouse position to raycast
        RaycastHit hit; 
        Vector3 clickedPosition;
        if (coinsN != null)
        {
            coinsN.text = coins.ToString();

        }
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
        {
            clickedPosition = hit.point;
            
            if (hit.collider.gameObject.tag == "Coin") {
               
                coins += 10;
                coinAudio.Play(0);
                Destroy(hit.collider.gameObject);

            }
            else {
                if (whichWeapon == 1 && coins >= 10)
                {
                    Instantiate(weapon1, new Vector3(clickedPosition.x, 1.5f, clickedPosition.z), Quaternion.identity);
                    coins -= weapon1.gameObject.GetComponent<WeaponScript>().price;
                }
                else if (whichWeapon == 2 && coins >= 20)
                {
                    coins -= weapon1.gameObject.GetComponent<WeaponScript>().price;
                    Instantiate(weapon2, new Vector3(clickedPosition.x, 1.5f, clickedPosition.z), Quaternion.identity);

                }
                else if (whichWeapon == 3 && coins >= 15) {
                    coins -= weapon1.gameObject.GetComponent<WeaponScript>().price;
                    Instantiate(weapon3, new Vector3(clickedPosition.x, 1.5f, clickedPosition.z), Quaternion.identity);
                }

            }
        }
    }
}