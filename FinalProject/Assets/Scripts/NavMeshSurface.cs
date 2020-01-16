using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NavMeshSurface : MonoBehaviour
{
    public Transform target;
    public Transform targetThirdScene;
    Transform newTarget;
    NavMeshAgent agent;
    public int speed = 10;
    Scene scene;

    void Start()
    {
         scene = SceneManager.GetActiveScene();

        if (scene.name == "ThirdScene")
            newTarget = targetThirdScene;
        else
            newTarget = target;

        agent = GetComponent<NavMeshAgent>();
    }
   
    void Update()
    {
        if (newTarget == null)
        {
            if (scene.name == "ThirdScene")
                newTarget = targetThirdScene;
            else
                newTarget = target;
        }
        agent.SetDestination(newTarget.position);
        agent.speed = speed;


        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, transform.position);
            if (dist < 10)
            {
                newTarget = enemy.transform;
            }
            
        }
    }
}
