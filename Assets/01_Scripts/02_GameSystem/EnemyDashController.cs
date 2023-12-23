
using System.Collections;
using UnityEngine;

public class EnemyDashController : MonoBehaviour
{
    public IEnemyController controller;
    [SerializeField] private int HitsToDash;

    [SerializeField] private int ActHits;
    [SerializeField] private float CoolDownTime;
    [SerializeField] private float PositionToDash;
    [SerializeField] private float DashTresholdStop;
    public Vector2 basePosition;
    private Vector2 dashPosition;
    
    public float MoveSpeed;
    public float SmoothTime;
    private Vector2 velocity;

    private bool DoDash;
    private void OnEnable()
    {
        if (controller == null)
            controller = GetComponent<IEnemyController>();
        ActHits = 0;
    }

    void CheckHits()
    {
        if (ActHits >= HitsToDash)
        {
            controller.ChangeState(State.Dash);
            StartCoroutine(Dash());
        }
    }

    private void Update()
    {
        if (DoDash)
        {
            transform.position = Vector2.SmoothDamp(transform.position, dashPosition, ref velocity, SmoothTime, MoveSpeed);
            if (Vector2.Distance(transform.position, dashPosition) < DashTresholdStop)
                DoDash = false;
        }
    }

    IEnumerator Dash()
    {
        dashPosition.x = transform.position.x;
        dashPosition.y = PositionToDash;
        DoDash = true;
        yield return ScriptsTools.GetWait(CoolDownTime);
        ActHits = 0;
        DoDash = false;
        transform.position = basePosition;
        controller.ChangeState(State.Movement);
    }

    

    public void AddHit()
    {
            ActHits++;
            CheckHits();
    }
}
