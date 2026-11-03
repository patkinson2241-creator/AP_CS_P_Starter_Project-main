using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    float Speed = 3.5f;
    float Xvector;
    float Yvector;
    float Xdirection;
    float Ydirection;
    Animator animator;
    SpriteRenderer renderer;
    int coins;
    int health = 3;
    int maxhealth = 5;
    bool cooldown = true;
    float cooldownTimer = 0.5f;
    GameObject currentEnemy;
    String facing;
    [SerializeField]GameObject fireball_prefab;
    [SerializeField]Transform spawnpoint;
    [SerializeField] GameObject Attackbox;

    private void Start()
    {
       animator = GetComponentInChildren<Animator>();
       renderer = GetComponentInChildren<SpriteRenderer>();
       Attackbox.SetActive(false);
    }

    private void Update()
    {
        
        Xdirection = Input.GetAxisRaw ("Horizontal");
        Ydirection = Input.GetAxisRaw ("Vertical");
        Xvector = Xdirection * Speed * Time.deltaTime;
        Yvector = Ydirection * Speed * Time.deltaTime;
        transform.position += new Vector3 (Xvector, Yvector, 0);
        AnimationUpdate();
        if(Input.GetKeyDown(KeyCode.E) && cooldownTimer <= 0 && currentEnemy != null)
        {
            currentEnemy.GetComponent<EnemyController>().ChangeHealth(-1.5f);
            cooldownTimer = 0.25f;
        }
        if(Input.GetKeyDown(KeyCode.Space) && cooldown)
        {
            Shoot();
            cooldownTimer = 0.5f;
        }
        if(Xdirection > 0)
        {
            facing = "Right";
            Attackbox.transform.position = transform.position + new Vector3(0.75f, 0.75f, 0);
        }
        else if(Xdirection < 0)
        {
            facing = "Left";
            Attackbox.transform.position = transform.position + new Vector3(-0.75f, 0.75f, 0);
        }
        else if(Ydirection > 0)
        {
            facing = "Up";
            Attackbox.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        }
        else if(Ydirection < 0)
        {
            facing = "Down";
            Attackbox.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        }
        if(cooldownTimer == 0.5f)
        {
            cooldown = false;
        }
        else if(cooldownTimer <= 0)
        {
            cooldown = true;
        }
        cooldownTimer -= Time.deltaTime;
    }

    void AnimationUpdate()
    {
        bool ismoving = Xvector != 0 || Yvector != 0;
        animator.SetBool("IsWalking", ismoving);
        if (Mathf.Abs(Xvector) > Mathf.Abs(Yvector))
        {
            animator.SetInteger("WalkDir", 1);
            if(Xvector > 0)
            {
                renderer.flipX = true;
            }
            else if(Xvector < 0)
            {
                renderer.flipX = false;
            }
        }
        else if(Yvector > 0)
        {
            animator.SetInteger("WalkDir", 0);
        }
        else if(Yvector < 0)
        {
            animator.SetInteger("WalkDir", 2);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Coin"))
        {
            ChangeCoins(1);
            Destroy(other.gameObject);
           
        }
        else if(other.gameObject.CompareTag("HealthPotion"))
        {
                ChangeHealth(1);
                Destroy(other.gameObject);
        }    
        else if(other.gameObject.CompareTag("E_Fireball"))
        {
           ChangeHealth(-2);
           Destroy(other.gameObject);
        }
        else if(other.gameObject.CompareTag("Enemy"))
        {
            currentEnemy = other.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            currentEnemy = other.gameObject;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Spikes"))
        {
            ChangeHealth(-1);
        }
    }

    void ChangeCoins(int amount)
    {
        coins += amount;  //Same as coins = coins + 1;
            
        print("You have "+ coins +" coins.");
    }

    void ChangeHealth(int amount)
    {
        health += amount;
        print("You have "+ health +" health.");
        if(health <= 0)
        {
            Die();
        }
        if(health > maxhealth)
        {
            health = maxhealth;
            print("You cannot gain any more health.");
            
        }
    }

    void Die()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
        print("You died!");
    }
    void Shoot()
    {
        GameObject fireball_copy = Instantiate(fireball_prefab, spawnpoint.position, quaternion.identity);
        PlayerProjectile projectile_script = fireball_copy.GetComponent<PlayerProjectile>();
        projectile_script.SetDirection(facing);
    }
}