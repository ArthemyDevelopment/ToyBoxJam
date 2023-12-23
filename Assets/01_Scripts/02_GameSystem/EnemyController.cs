
using System;
using System.Collections;
using UnityEngine;

public enum State
{
    Movement,
    Dash,
}

public interface IEnemyController
{
    public void ChangeState(State state);
    public State GetState();
    public void PlayHitAnim();

}

public class EnemyController : MonoBehaviour, IEnemyController
{

    public float MoveSpeed;
    public float RefreshPositionTime;
    public float SmoothTime;
    private Vector2 Target;
    private Vector2 velocity;
    private bool IsActive;

    
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
            Target.x = BallController.current.gameObject.transform.position.x;
            Target.y = transform.position.y;
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
                anim.Play("SnowmanDash");
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
