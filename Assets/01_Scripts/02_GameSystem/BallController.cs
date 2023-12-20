using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class BallController : SingletonManager<BallController>
{
    private Rigidbody2D rb;
    public float StartSpeed;
    public float SpeedMultiplier;
    public float SpeedAdditive;
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
        //Debug.Log(rb.velocity);
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
        
        if(col.CompareTag("Player"))
        {
            
            Vector2 Dir = transform.position - PlayerController.current.transform.position;
            Bounce(col,Dir);
            
        }   
        else if (col.CompareTag("Speed"))
        {
            if (col.gameObject.transform.position.y - 0.2 < transform.position.y) return;
            Vector2 Dir = Vector2.down.normalized;
            Bounce(col,Dir);
            col.GetComponent<EnemyDashController>().AddHit();
        }
    }

    /*private void OnTriggerStay(Collider col)
    {
        if(col.CompareTag("Player Stay"))
        {
            Debug.Log("Trigger Player stay");
           
            
            
        } 
    }*/

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Score"))
        {
            
            GameManager.current.AddScore();
            ActSpeed = (ActSpeed* SpeedMultiplier) + SpeedAdditive;
        }
        else if (col.gameObject.CompareTag("GameOver"))
        {
            
            rb.velocity = Vector2.zero;
        }
    }

    public void Bounce(Collider2D col, Vector2 Dir)
    {
        
        rb.velocity = Dir.normalized * ActSpeed ;
        
    }
}
