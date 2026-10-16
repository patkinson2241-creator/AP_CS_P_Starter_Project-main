using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    float speed = 2.75f;
    Transform target;
    Vector3 target_position;
    bool inRange = false;
    bool inAttackRange = false;
    public void Start()
    {
        
    }

    public void Update()
    {
        target_position = target.position;
        if(inRange)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target_position, step);
        }
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