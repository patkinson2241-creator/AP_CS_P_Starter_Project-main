using System;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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
    float cooldownTimer = 0.75f;
    String facing;
    [SerializeField]GameObject fireball_prefab;
    [SerializeField]Transform spawnpoint;

    private void Start()
    {
       animator = GetComponentInChildren<Animator>();
       renderer = GetComponentInChildren<SpriteRenderer>();
       
       
    }

    private void Update()
    {
        
        Xdirection = Input.GetAxisRaw ("Horizontal");
        Ydirection = Input.GetAxisRaw ("Vertical");
        Xvector = Xdirection * Speed * Time.deltaTime;
        Yvector = Ydirection * Speed * Time.deltaTime;
        transform.position += new Vector3 (Xvector, Yvector, 0);
        AnimationUpdate();
       if(Input.GetKeyDown(KeyCode.Space) && cooldown)
        {
            Shoot();
        }
        if(Xdirection > 0)
        {
            facing = "Right";
        }
        else if(Xdirection < 0)
        {
            facing = "Left";
        }
        else if(Ydirection > 0)
        {
            facing = "Up";
        }
        else if(Ydirection < 0)
        {
            facing = "Down";
        }
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
        if (other.gameObject.CompareTag("Coin"))
        {
            ChangeCoins(1);
            Destroy(other.gameObject);
           
        }
        else if (other.gameObject.CompareTag("HealthPotion"))
        {
                ChangeHealth(1);
                Destroy(other.gameObject);
        }    
        else if (other.gameObject.CompareTag("E_Fireball"))
        {
           ChangeHealth(-2);
           Destroy(other.gameObject);
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
            Debug.Log(currentScene);
            SceneManager.LoadScene(currentScene);
            print("You died!");
    }
    void Shoot()
    {
        GameObject fireball_copy = Instantiate(fireball_prefab, spawnpoint.position, quaternion.identity);
        PlayerProjectile projectile_script = fireball_copy.GetComponent<PlayerProjectile>();
    }
}