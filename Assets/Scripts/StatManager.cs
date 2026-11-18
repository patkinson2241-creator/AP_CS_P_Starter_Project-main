using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class StatManager : MonoBehaviour
{
    public int health = 3;
    public int maxhealth = 5;
    public int coins;
    public int maxjumps = 1;
    public static StatManager original;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI healthText;
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
    void Start()
    {
        healthText.text = "Health: "+ health +"/"+ maxhealth +"";
        coinText.text = "Coins: "+coins+"";
    }
    public void ChangeCoins(int amount)
    {
        coins += amount;  //Same as coins = coins + 1;
            
        coinText.text = "Coins: "+coins+"";
    }

    public void ChangeHealth(int amount)
    {
        health += amount;
        if(health <= 0)
        {
            Die();
        }
        if(health > maxhealth)
        {
            health = maxhealth;
            print("You cannot gain any more health. Health converted to coins.");
            ChangeCoins(1);
        }
        healthText.text = "Health: "+ health +"/"+ maxhealth +"";
    }

    public void Die()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
        print("You died!");
    }
}
