using System.Collections;
using UnityEngine;

public class EnemyImitateController : MonoBehaviour, IEnemyController
{
    public float MoveSpeed;
    public float RefreshPositionTime;
    public float SmoothTime;
    private Vector2 Target;
    private Vector2 velocity;
    private bool IsActive;

    
    [SerializeField]private State ActState;



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
            Target.x = PlayerController.current.PositionX;
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
    public State GetState()
    {
        return ActState;
    }
}
