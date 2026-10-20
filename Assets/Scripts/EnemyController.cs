using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    float speed = 1.5f;
    Transform target;
    Vector3 target_position;
    bool inRange = false;
    bool inAttackRange = false;
    int patrolNumber = 0;
    [SerializeField] private List<Transform> patrolpoints = new List<Transform>();
    public void Start()
    {
        target_position = patrolpoints[patrolNumber].position;
    }

    public void Update()
    {
        
        if(inRange)
        {
            target_position = target.position;
            speed = 2.25f;
        }
        else
        {
            speed = 1.5f;
            target_position = patrolpoints[patrolNumber].position;
            if(Vector3.Distance(transform.position, patrolpoints[patrolNumber].position)<.1)
            {
                patrolNumber += 1;
                if(patrolNumber == patrolpoints.Count)
                {
                    patrolNumber = 0;
                }
            }
            target_position = patrolpoints[patrolNumber].position;
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target_position, step);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            inRange = true;
            target = other.gameObject.transform;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            inRange = false;
            target = null;
        }
    }
}