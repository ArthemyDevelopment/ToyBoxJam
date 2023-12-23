
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerController : SingletonManager<PlayerController>
{

    public GameObject HitBox;
    public Rigidbody2D HitBoxRb;
    public float timeHitBox;
    public float PositionX;



    enum MoveType
    {
        Instant,
        Delay,
        Lose,
    }

    [SerializeField]private MoveType ActMoveType;
    
    public float MoveSpeed;
    public float SmoothTime;
    private Vector2 Target;
    private Vector2 velocity;
    [SerializeField] private float SlowTime;

  
#if UNITY_WEBGL
    private Vector3 ScreenPoint;
#elif UNITY_ANDROID
    private Vector2 ScreenPoint;
#endif
    
    
    public Animator anim;

    public void OnEnable()
    {
        if (anim == null) anim = GetComponent<Animator>();
        ActiveMovement();
        ScreenPoint = transform.position;


    }

    // Update is called once per frame
    void Update()
    {
        switch (ActMoveType)
        {
            case MoveType.Instant:
                InstantMove();
                break;
            case MoveType.Delay:
                DelayMove();
                break;
            case MoveType.Lose:
                break;

        }
        if(ActMoveType!=MoveType.Lose)
            HitBallInput();
    }


    void HitBallInput()
    {
#if UNITY_WEBGL
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ActiveHitBox()); 
            anim.Play("HitBall");
        }
#endif
        
#if UNITY_ANDROID
       if (Input.touchCount>0)
       {
           if (Input.GetTouch(0).phase == TouchPhase.Began)
           {
                StartCoroutine(ActiveHitBox());
                anim.Play("HitBall");
           }
        }
#endif
        
    }
    
    void InstantMove()
    {
#if UNITY_WEBGL
        ScreenPoint = Input.mousePosition;
        ScreenPoint.z = 10; 
#endif

#if UNITY_ANDROID
        
        if(Input.touchCount>0)
            if(Input.GetTouch(0).phase==TouchPhase.Moved||Input.GetTouch(0).phase==TouchPhase.Stationary)
                ScreenPoint = Input.GetTouch(0).position;
#endif
        PositionX = Camera.main.ScreenToWorldPoint(ScreenPoint).x;
        transform.position = new Vector2(PositionX, transform.position.y);

    }

    void DelayMove()
    {
        Target.y = transform.position.y;
        
        #if UNITY_WEBGL
        
            ScreenPoint = Input.mousePosition;
            ScreenPoint.z = 10; 
        
        #endif
        
        #if UNITY_ANDROID
        
        if(Input.touchCount>0)
            if(Input.GetTouch(0).phase==TouchPhase.Moved||Input.GetTouch(0).phase==TouchPhase.Stationary)
                ScreenPoint = Input.GetTouch(0).position;
        
        #endif
        
        var positionX = Camera.main.ScreenToWorldPoint(ScreenPoint).x;
        Target.x = positionX; 
        transform.position = Vector2.SmoothDamp(transform.position, Target, ref velocity, SmoothTime, MoveSpeed);
    }

    IEnumerator ActiveHitBox()
    {
        HitBox.SetActive(true);

        yield return ScriptsTools.GetWait(timeHitBox);
        HitBox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Speed")) return;

        if (col.GetComponent<IEnemyController>().GetState() != State.Dash) return;

        ActMoveType = MoveType.Delay;
        StartCoroutine(SlowDebuffDuration());
    }

    IEnumerator SlowDebuffDuration()
    {
        yield return ScriptsTools.GetWait(SlowTime);
        ActMoveType = MoveType.Instant;
    }

    public void Lose()
    {
        anim.Play("Lose");
        ActMoveType = MoveType.Lose;
    }

    public void ActiveMovement()
    {
        ActMoveType = MoveType.Instant;
        anim.Play("Idle");
    }

}
