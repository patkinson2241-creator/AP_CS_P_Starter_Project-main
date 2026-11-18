using UnityEditor;
using UnityEngine;

public class Boots : MonoBehaviour
{
    StatManager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(manager.maxjumps > 1)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
