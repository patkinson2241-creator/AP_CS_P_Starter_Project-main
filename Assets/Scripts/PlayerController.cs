using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEditor.Callbacks;
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
    int jumpsleft;
    bool platformer;
    bool onladder;
    Scene currentScene;
    GameObject currentEnemy;
    String facing;
    [SerializeField]GameObject fireball_prefab;
    [SerializeField]Transform spawnpoint;
    [SerializeField] GameObject Attackbox;
    StatManager manager;
    Rigidbody2D rb;
    [SerializeField] float jumpForce;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        manager = FindAnyObjectByType<StatManager>();
        animator = GetComponentInChildren<Animator>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        Attackbox.SetActive(false);
        currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "Overworld")
        {
            rb.gravityScale = 0;
            platformer = false;
        }
        else
        {
            rb.gravityScale = 2;
            platformer = true;
        }
        jumpsleft = manager.maxjumps;
    }

    private void Update()
    {
        
        Xdirection = Input.GetAxisRaw ("Horizontal");
        Xvector = Xdirection * Speed * Time.deltaTime;
        
        AnimationUpdate();
        if(Input.GetKeyDown(KeyCode.E) && cooldownTimer <= 0 && currentEnemy != null)
        {
            currentEnemy.GetComponent<EnemyController>().ChangeHealth(-1.5f);
            cooldownTimer = 0.25f;
        }
        if(Input.GetKeyDown(KeyCode.R) && cooldown)
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
        transform.position += new Vector3 (Xvector, 0, 0);
        if(!platformer)
        {
            Ydirection = Input.GetAxisRaw ("Vertical");
            Yvector = Ydirection * Speed * Time.deltaTime;
            
            if(Ydirection > 0)
            {
            facing = "Up";
            Attackbox.transform.position = transform.position + new Vector3(0, 1.5f, 0);
            }
            else if(Ydirection < 0)
            {
                facing = "Down";
                Attackbox.transform.position = transform.position + new Vector3(0, -0.5f, 0);
            }
            transform.position += new Vector3 (Xvector, Yvector, 0);
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
       
        if((Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.UpArrow)) && jumpsleft >= 1 && platformer)
        {
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            jumpsleft -= 1;
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
        if(other.gameObject.CompareTag("Coin"))
        {
            manager.ChangeCoins(1);
            Destroy(other.gameObject);
           
        }
        else if(other.gameObject.CompareTag("HealthPotion"))
        {
            manager.ChangeHealth(1);
            Destroy(other.gameObject);
        }    
        else if(other.gameObject.CompareTag("E_Fireball"))
        {
           manager.ChangeHealth(-2);
           Destroy(other.gameObject);
        }
        else if(other.gameObject.CompareTag("Enemy"))
        {
            currentEnemy = other.gameObject;
        }
        else if(other.gameObject.CompareTag("Boots"))
        {
            Destroy(other.gameObject);
            manager.maxjumps = 2;
            jumpsleft = manager.maxjumps;
        }
        else if(other.gameObject.CompareTag("Lader"))
        {
            platformer = false;
            onladder = true;
            rb.gravityScale = 0;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            currentEnemy = other.gameObject;
        }
        else if(other.gameObject.CompareTag("Lader"))
        {
            platformer = true;
            onladder = false;
            rb.gravityScale = 2;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Spikes"))
        {
            manager.ChangeHealth(-1);
        }
        else if(other.gameObject.CompareTag("Ground"))
        {
            jumpsleft = manager.maxjumps;
        }
        else if(other.gameObject.CompareTag("Platform"))
        {
        
        }
    }
    void Shoot()
    {
        GameObject fireball_copy = Instantiate(fireball_prefab, spawnpoint.position, quaternion.identity);
        PlayerProjectile projectile_script = fireball_copy.GetComponent<PlayerProjectile>();
        projectile_script.SetDirection(facing);
    }
}