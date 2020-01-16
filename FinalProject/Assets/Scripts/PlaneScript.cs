using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaneScript : MonoBehaviour
{
    public GameObject EnemyLevelOne;
    public GameObject EnemyLevelTwo;
    public GameObject coin;
    bool canCreateEnemy = true;
    bool canCreateCoin = true;
    public int time = 5; //delay Time
    public int timeForCoin = 10;
    public int numberOfEnemy = 3;
    public Text text;

    public Button nextLevel;

    private void Start()
    {
        nextLevel.gameObject.SetActive(false);
        nextLevel.onClick.AddListener(GoToNExtLevel);
    }

    private void GoToNExtLevel()
    {
        SceneManager.LoadScene("ThirdScene");

    }



    void Update()
    {
        if (numberOfEnemy == 0) {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                text.gameObject.SetActive(true);
                text.text = "You Won";
                Time.timeScale = 0;
                nextLevel.gameObject.SetActive(true);

            }
        }
        if (canCreateEnemy && numberOfEnemy > 0)
        {
            float randX = (UnityEngine.Random.Range(0.0f, 47.0f));
            int randEnenmy = UnityEngine.Random.Range(1,3);
            print(randEnenmy);
            StartCoroutine(ExecuteAfterTime(time, randX, randEnenmy));
            canCreateEnemy = false;
        }
        if (canCreateCoin)
        {
            float randX = (UnityEngine.Random.Range(0.0f, 45.0f));
            float randZ = (UnityEngine.Random.Range(0.0f, -55.0f));
            StartCoroutine(ExecuteAfterTimeCoin(timeForCoin, randX, randZ));
            canCreateCoin = false;
        }
    }
    IEnumerator ExecuteAfterTime(float time, float x, int randEnenmy)
    {
        yield return new WaitForSeconds(time);
        if(randEnenmy == 1)
             Instantiate(EnemyLevelOne, new Vector3(x, 2f, -1.0f), Quaternion.identity);
        else if(randEnenmy == 2)
            Instantiate(EnemyLevelTwo, new Vector3(x, 2f, -1.0f), Quaternion.identity);

        canCreateEnemy = true;
        --numberOfEnemy;

    }
    IEnumerator ExecuteAfterTimeCoin(float time, float x, float z)
    {
        yield return new WaitForSeconds(time);
        Instantiate(coin, new Vector3(x, 2f, z), Quaternion.Euler(0, 0, 60));
        canCreateCoin = true;
    }
}
