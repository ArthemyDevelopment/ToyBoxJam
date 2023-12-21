
using System;
using System.Collections;
using UnityEngine;


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
    }

    [SerializeField]private MoveType ActMoveType;
    
    public float MoveSpeed;
    public float SmoothTime;
    private Vector2 Target;
    private Vector2 velocity;
    [SerializeField] private float SlowTime;
    
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

        }
        HitBallInput();
    }


    void HitBallInput()
    {
#if UNITY_WEBGL
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ActiveHitBox()); 
        }
#endif
        
#if UNITY_ANDROID
       if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ActiveHitBox());
        }
#endif
        
    }
    
    void InstantMove()
    {
#if UNITY_WEBGL
        
        var ScreenPoint = Input.mousePosition;
        ScreenPoint.z = 10;
        PositionX = Camera.main.ScreenToWorldPoint(ScreenPoint).x;
        transform.position = new Vector2(PositionX, transform.position.y);
#endif

#if UNITY_ANDROID
        var ScreenPoint = Input.GetTouch[0].position;
        ScreenPoint.z = 10;
        PositionX = Camera.main.ScreenToWorldPoint(ScreenPoint).x;
         transform.position = new Vector2(PositionX, transform.position.y);
#endif

    }

    void DelayMove()
    {
        
        #if UNITY_WEBGL
        Target.y = transform.position.y;
        var ScreenPoint = Input.mousePosition;
        ScreenPoint.z = 10;
        var positionX = Camera.main.ScreenToWorldPoint(ScreenPoint).x;
        Target.x = positionX; 
        transform.position = Vector2.SmoothDamp(transform.position, Target, ref velocity, SmoothTime, MoveSpeed);
        
        #endif
    }

    IEnumerator ActiveHitBox()
    {
        HitBox.SetActive(true);
        //HitBoxRb.WakeUp();
        yield return ScriptsTools.GetWait(timeHitBox);
        HitBox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name);
        if (!col.CompareTag("Speed")) return;
        Debug.Log(col.GetComponent<IEnemyController>().GetState());
        if (col.GetComponent<IEnemyController>().GetState() != State.Dash) return;

        ActMoveType = MoveType.Delay;
        StartCoroutine(SlowDebuffDuration());
    }

    IEnumerator SlowDebuffDuration()
    {
        yield return ScriptsTools.GetWait(SlowTime);
        ActMoveType = MoveType.Instant;
    }

}
