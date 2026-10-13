using System;
using System.Threading;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    float speed = 20;
    Vector3 direction;
    float timer = 3.5f;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void SetDirection(String facing)
    {
        if(facing == "Right")
        {
            direction = new Vector3(1,0,0);
        }
        else if(facing == "Left")
        {
            direction = new Vector3(-1,0,0);
        }
        else if(facing == "Up")
        {
            direction = new Vector3(0,1,0);
        }
        else if(facing == "Down")
        {
            direction = new Vector3(0,-1,0);
        }
    }
}
