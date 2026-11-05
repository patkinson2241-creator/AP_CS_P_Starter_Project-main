using UnityEngine;
using UnityEngine.SceneManagement;
public class StatManager : MonoBehaviour
{
    int health = 3;
    int maxhealth = 5;
    int coins;
    public static StatManager original;
    void Awake()
    {
        if(original != null && original != this)
        {
            Destroy(gameObject);
            return;
        }
        original = this;
        DontDestroyOnLoad(gameObject);
    }
    public void ChangeCoins(int amount)
    {
        coins += amount;  //Same as coins = coins + 1;
            
        print("You have "+ coins +" coins.");
    }

    public void ChangeHealth(int amount)
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

    public void Die()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
        print("You died!");
    }
}
