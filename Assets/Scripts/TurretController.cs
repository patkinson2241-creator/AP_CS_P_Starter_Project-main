using System;
using Unity.Mathematics;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    bool inRange = false;
    float timer;
    float time_between_shots = 1.5f;
    [SerializeField]GameObject fireball_prefab;
    [SerializeField]Transform spawnpoint;
    Transform target;
    private void Start()
    {
       timer = time_between_shots;
    }

    private void Update()
    {
        if(inRange)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                Shoot();
                timer = time_between_shots;
            }
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
    void Shoot()
    {
        GameObject fireball_copy = Instantiate(fireball_prefab, spawnpoint.position, quaternion.identity);
        Projectile projectile_script = fireball_copy.GetComponent<Projectile>();
        projectile_script.target = target;
    }
}