using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TapScriptLevelOne : MonoBehaviour
{
    public Button btn1;
   
    public Image img1;
    
    public Button menu;
    public Button pause;
    bool isPause = false;
    public GameObject weapon1;
   
    public int price = 5;
    int whichWeapon = 0;
    public int coins = 10;
    public Text coinsN;
    public AudioSource Music;
    public AudioSource coinSound;


    private void Start()
    {
        
        Music.Play(0);
        menu.gameObject.SetActive(false);

        img1.gameObject.SetActive(false);
        
        btn1.onClick.AddListener(UseWeapon1);
        
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
                coinSound.Play(0);
                Destroy(hit.collider.gameObject);
            }
            else {
                if (whichWeapon == 1 && coins >= 10)
                {
                    Instantiate(weapon1, new Vector3(clickedPosition.x, 1.5f, clickedPosition.z), Quaternion.identity);
                    coins -= weapon1.gameObject.GetComponent<WeaponScript>().price;
                }
               

            }
        }
    }
}