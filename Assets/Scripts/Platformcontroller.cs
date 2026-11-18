using UnityEngine;
using System.Collections.Generic;
public class Platformcontroller : MonoBehaviour
{
    Vector3 target_position;
    float speed = 2.5f;
    int patrolNumber = 0;
    [SerializeField] private List<Transform> patrolpoints = new List<Transform>(); 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target_position = patrolpoints[patrolNumber].position;
    }

    // Update is called once per frame
    void Update()
    {
        target_position = patrolpoints[patrolNumber].position;
        if(Vector3.Distance(transform.position, patrolpoints[patrolNumber].position)<.1)
        {
            {
                patrolNumber += 1;
                if(patrolNumber == patrolpoints.Count)
                {
                    patrolNumber = 0;
                }
            }
        }
        target_position = patrolpoints[patrolNumber].position;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target_position, step);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.SetParent(gameObject.transform);
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.SetParent(null);
        }
    }
}
