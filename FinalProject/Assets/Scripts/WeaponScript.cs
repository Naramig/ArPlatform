using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public GameObject Bullet;
    GameObject newBullet;
    bool canCreate = true;
    public int Health = 100;
    public float speedOfShooting = 1;
    public int distance = 10;
    public int takeHealthFromEnemy = 25;
    public int price = 10;
    
    void Update()
    {

        if (Health <= 0)
        {
            Destroy(gameObject);
            
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, transform.position);
            
            if (dist < distance) {
                if (canCreate)
                {
                    StartCoroutine( ExecuteAfterTime(speedOfShooting, enemy)); //starts creating bullet and shoots
                    canCreate = false;
                }
            }
        }
    }
    IEnumerator ExecuteAfterTime(float time, GameObject enemy)
    {
        yield return new WaitForSeconds(time);
        if (enemy != null)
        {
            newBullet = Instantiate(Bullet, new Vector3(this.transform.position.x, 3.5f, this.transform.position.z), Quaternion.identity);
            newBullet.GetComponent<BulletScript>().takeHealth = takeHealthFromEnemy; //assign how many points of health take from enemy
            newBullet.GetComponent<BulletScript>().enemie = enemy; //assign enemy GameObject for position
        }
        canCreate = true;
    }


}