using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1EnemyScript : MonoBehaviour
{
    
    public int Health = 100;
    bool canAttack = true;
    public int costToKill = 5;
    public int TakeHealth = 25;
    GameObject camera;
    public ParticleSystem exp;
    public AudioSource explod;

    void Update()
    {
        if (Health <= 0)
        {
            Explode();

            camera = GameObject.FindGameObjectWithTag("MainCamera");
            Scene scene = SceneManager.GetActiveScene();
            if(scene.name == "SecondScene")
                 camera.GetComponent<TapScript>().coins += costToKill;
            else if(scene.name == "FirstScene")
                 camera.GetComponent<TapScriptLevelOne>().coins += costToKill;
            else if (scene.name == "ThirdScene")
               camera.GetComponent<TapScript>().coins += costToKill;

        }

        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject weapon in weapons)
        {
            float dist = Vector3.Distance(weapon.transform.position, transform.position);
            if (dist < 1 && canAttack)
            {
                
                StartCoroutine(ExecuteAfterTime(1, weapon));
                canAttack = false;
            }

        }

    }
    void Explode()
    {

        ParticleSystem exp1 = Instantiate(exp, new Vector3(this.transform.position.x, 3.5f, this.transform.position.z), Quaternion.identity);
        exp1.Play();
        explod.Play(0);

        Destroy(gameObject, exp1.duration);
        Destroy(gameObject);

    }
    IEnumerator ExecuteAfterTime(float time, GameObject weapon)
    {
        yield return new WaitForSeconds(time);
        if (weapon != null)
        {
            weapon.gameObject.GetComponent<WeaponScript>().Health -= TakeHealth;

        }
        canAttack = true;
    }

}
