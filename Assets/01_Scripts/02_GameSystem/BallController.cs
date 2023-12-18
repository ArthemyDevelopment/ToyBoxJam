using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : SingletonManager<BallController>
{
    private Rigidbody2D rb;
    public float StartSpeed;
    public float SpeedModifier;
    [SerializeField]private float ActSpeed;
    
    
    private void OnEnable()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    [Button]
    public void InitBall()
    {
        float xVelocity = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(xVelocity, -1).normalized* StartSpeed;
        Debug.Log(rb.velocity);
        ActSpeed = StartSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }


    /*private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Speed"))
        {            
            
        }
        else if(col.CompareTag("Score"))
        else if (col.CompareTag("GameOver"))
    }
    */

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Speed"))
        {
            if (col.gameObject.transform.position.y - 0.3 < transform.position.y) return;
            
            BounceAndSpeed(col);
        }
        else if(col.CompareTag("Player"))
        {
            if (col.gameObject.transform.position.y > transform.position.y) return;
            
            BounceAndSpeed(col);
            
        }   
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Score"))
        {
            //Debug.Log("Player Score");
            GameManager.current.AddScore();
        }
        else if (col.gameObject.CompareTag("GameOver"))
        {
            //Debug.Log("GameOver");
            rb.velocity = Vector2.zero;
        }
    }

    public void BounceAndSpeed(Collider2D col)
    {
        //Debug.Log("Speed up");
        Vector2 Dir = transform.position - col.gameObject.transform.position;
        ActSpeed *= SpeedModifier;
        rb.velocity = Dir.normalized * ActSpeed ;
        //Debug.Log(rb.velocity);
    }
}
