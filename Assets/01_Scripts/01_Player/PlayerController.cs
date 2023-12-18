using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private float inputHorizontal;
    public Vector2 HorMove;
    public float MoveSpeed;
    private Rigidbody2D rb;
    private void OnEnable()
    {
        if(rb==null)
            rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();    
    }

    private void FixedUpdate()
    {
        rb.velocityX = HorMove.x * MoveSpeed;
    }

    void CheckInput()
    {
#if UNITY_WEBGL
        
        inputHorizontal = Input.GetAxis("Horizontal");
        HorMove.x = inputHorizontal;
#endif
        
    }

    public void MoveLeft()
    {
        HorMove.x += -1;
    }

    public void StopLeft()
    {
        HorMove.x += 1;
    }

    public void MoveRight()
    {
        HorMove.x += 1;
    }

    public void StopRight()
    {
        HorMove.x += -1;
    }
}
