using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Movement,
    Dash,
}

public interface IEnemyController
{
    public void ChangeState(State state);

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
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
        if (state == State.Movement)
            StartCoroutine(RefreshMovement());
    }
}
