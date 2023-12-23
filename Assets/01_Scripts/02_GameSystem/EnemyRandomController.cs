using System.Collections;
using UnityEngine;

public class EnemyRandomController : MonoBehaviour, IEnemyController
{
    public float MoveSpeed;
    public float RefreshPositionTime;
    public float SmoothTime;
    private Vector2 Target;
    private Vector2 velocity;
    private bool IsActive;
    public Transform[] ScreenBorderReference = new Transform[2];
    public float MinMoveDistance;

    
    [SerializeField]private State ActState;
    
    public Animator anim;


    private void OnEnable()
    {
        if (anim == null) anim = GetComponent<Animator>();
        ActState = State.Movement;
        StartCoroutine(RefreshMovement());
        IsActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(ActState==State.Dash) return;

        transform.position = Vector2.SmoothDamp(transform.position, Target, ref velocity, SmoothTime, MoveSpeed);
    }

    IEnumerator RefreshMovement()
    {
        while (ActState== State.Movement)
        {
            float tempPosition = 0;
            float moveDistance = 0;
            while (moveDistance < MinMoveDistance)
            {
                tempPosition = Random.Range(ScreenBorderReference[0].position.x, ScreenBorderReference[1].position.x);
                Target.x = tempPosition;
                Target.y = transform.position.y;
                moveDistance = Vector2.Distance(transform.position, Target);
            }


            yield return ScriptsTools.GetWait(RefreshPositionTime);
        }
    }

    public void ChangeState(State state)
    {
        ActState = state;
        switch (state)
        {
            case State.Movement:
                StartCoroutine(RefreshMovement());
                anim.Play("EnemyIdle");
                break;
            case State.Dash:
                anim.Play("BunnyDash");
                break;
            
        }
    }
    public State GetState()
    {
        return ActState;
    }

    public void PlayHitAnim()
    {
        anim.Play("EnemyHit");
    }
}
