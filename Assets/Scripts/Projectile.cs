using UnityEngine;

public class Projectile : MonoBehaviour
{
    float timer;
    float time_between_shots = 1.5f;
    float speed = 12;
    public Transform target;
    Vector3 target_position;
    private void Start()
    {
        timer = time_between_shots;
        target_position = target.position;
    }


    private void Update()
    {
       float step = speed * Time.deltaTime;
       transform.position = Vector3.MoveTowards(transform.position, target_position, step);
         timer -= Time.deltaTime;
        if(timer < 0)
        {
            Destroy(gameObject);
        }
    }
}
