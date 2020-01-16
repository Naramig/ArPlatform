using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Button ok;
    public Text weapon;
    public Text weaponInfo;
    public Text pause;
    public Text coin;
    public Text enemy;
    int n = 0;
    void Start()
    {
        ok.gameObject.SetActive(false);
        weapon.gameObject.SetActive(false);
        weaponInfo.gameObject.SetActive(false);
        pause.gameObject.SetActive(false);
        coin.gameObject.SetActive(false);
        enemy.gameObject.SetActive(false);

        ok.onClick.AddListener(OkClick);

        StartCoroutine(ExecuteAfterTime(weapon.gameObject, 1));
    }
    void OkClick()
    {
        ok.gameObject.SetActive(false);
        weapon.gameObject.SetActive(false);
        weaponInfo.gameObject.SetActive(false);
        pause.gameObject.SetActive(false);
        coin.gameObject.SetActive(false);
        enemy.gameObject.SetActive(false);

        Time.timeScale = 1;

        switch (n) {
            case 1:
                StartCoroutine(ExecuteAfterTime(weaponInfo.gameObject, 1));
                break;
            case 2:
                StartCoroutine(ExecuteAfterTime(pause.gameObject, 1));
                break;
            case 3:
                StartCoroutine(ExecuteAfterTime(enemy.gameObject, 2));
                break;
            case 4:
                StartCoroutine(ExecuteAfterTime(coin.gameObject, 5));
                break;
        }
    }
    IEnumerator ExecuteAfterTime(GameObject text, int time)
    {
        yield return new WaitForSeconds(time);
        text.gameObject.SetActive(true);
        ok.gameObject.SetActive(true);
        Time.timeScale = 0;
        ++n;

    }
}
