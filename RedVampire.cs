using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedVampire : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float flyDistance;
    private bool facingLeft = true;

    protected override void Start()
    {
        base.Start();
    }
    private void Update()
    {
        if(facingLeft)
        {
            
            if (transform.position.x > leftCap)
            {
                
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector2(1, 1);
                }
                rb.velocity = new Vector2(-flyDistance, rb.velocity.y);
            }
            
            else
            {
                facingLeft = false;
     
            }
        }
        else
        {
            
            if (transform.position.x < rightCap)
            {
                
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector2(-1, 1);
                }
                rb.velocity = new Vector2(flyDistance, rb.velocity.y);
            }
    
            else
            {
                facingLeft = true;
              
            }
        }
    }
    
}
