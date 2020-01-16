using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
   public GameObject enemie;
   public int takeHealth = 25;
    void Update()
    {
        float step = 10 * Time.deltaTime; // calculate distance to move
        if (enemie == null)
            Destroy(gameObject);
        else
             transform.position = Vector3.MoveTowards(transform.position, enemie.transform.position, step);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != null && collision.gameObject.tag == "Enemy")
        {
             
            collision.gameObject.GetComponent<Level1EnemyScript>().Health -= takeHealth;
            Destroy(gameObject);

        }
    }
}
