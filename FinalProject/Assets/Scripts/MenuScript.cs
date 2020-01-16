using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Button startGame;
    public Button exitGame;

    public Button YesExit;
    public Button NoExit;

    public Button goToLevelOne;
    public Button goToLevelTwo;
    public Button goToLevelThree;
    public Text ask;

    public Button showLevels;
    bool show = false;

    // Start is called before the first frame update
    void Start()
    {
        showLevels.onClick.AddListener(ShowAllLevels);
        ask.gameObject.SetActive(false);
        YesExit.gameObject.SetActive(false);
        NoExit.gameObject.SetActive(false);

        startGame.onClick.AddListener(StartNewGame);

        exitGame.onClick.AddListener(AskExitGame);
        YesExit.onClick.AddListener(ExitGame);
        NoExit.onClick.AddListener(NoExitGame);

        goToLevelOne.gameObject.SetActive(false);
        goToLevelTwo.gameObject.SetActive(false);
        goToLevelThree.gameObject.SetActive(false);

        goToLevelOne.onClick.AddListener(GoToLevelOne);
        goToLevelTwo.onClick.AddListener(GoToLevelTwo);
        goToLevelThree.onClick.AddListener(GoToLevelThree);



    }
    void ShowAllLevels()
    {
        if (show)
        {
            show = false;
            goToLevelOne.gameObject.SetActive(false);
            goToLevelTwo.gameObject.SetActive(false);
            goToLevelThree.gameObject.SetActive(false);
        }
        else
        {
            goToLevelOne.gameObject.SetActive(true);
            goToLevelTwo.gameObject.SetActive(true);
            goToLevelThree.gameObject.SetActive(true);
            show = true;
        }
       }
    private void GoToLevelThree()
    {
        SceneManager.LoadScene("ThirdScene");
    }

    private void GoToLevelTwo()
    {
        SceneManager.LoadScene("SecondScene");
    }

    private void GoToLevelOne()
    {
        SceneManager.LoadScene("FirstScene");

    }

    void NoExitGame()
    {
        YesExit.gameObject.SetActive(false);
        NoExit.gameObject.SetActive(false);
        ask.gameObject.SetActive(false);

    }
    void ExitGame()
    {
        Application.Quit();

    }
    private void StartNewGame()
    {
        SceneManager.LoadScene("FirstScene");

    }
    private void AskExitGame()
    {
        ask.gameObject.SetActive(true);
        YesExit.gameObject.SetActive(true);
        NoExit.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            AskExitGame();
        }
        
    }
}
