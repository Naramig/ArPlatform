using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HouseScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    public Button retry; 

    void Start()
    {
        Time.timeScale = 1;

        text.gameObject.SetActive(false);
        retry.gameObject.SetActive(false);
        retry.onClick.AddListener(RetryLevel);

        
    }


    // Update is called once per frame
    void Update()
    {
        print(Time.timeScale);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, transform.position);
            if (dist < 2)
            {
                text.gameObject.SetActive(true);
                text.text = "Game over";
                Time.timeScale = 0;
                retry.gameObject.SetActive(true);

            }
        }
    }
    private void RetryLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }

}
