
using System.Collections;
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
    private bool CDBounce;
    private Vector2 StartPosition;
    public Animator anim;

    public AudioSource HitSFX;
    public AudioSource ScoreSFX;
    public AudioSource GameOverSFX;


    private void OnEnable()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        
        StartPosition = transform.position;
    }

    [Button]
    public void InitBall()
    {
        transform.position = StartPosition;
        float xVelocity = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(xVelocity, -1).normalized* StartSpeed;
        ActSpeed = StartSpeed;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if(col.CompareTag("Player"))
        {
            HitSFX.Play();
            anim.Play("OnHit");
            Vector2 Dir = transform.position - PlayerController.current.transform.position;
            Bounce(col,Dir);
            
        }   
        else if (col.CompareTag("Speed"))
        {
            if (col.gameObject.transform.position.y - 0.2 < transform.position.y) return;
            IEnemyController temp = col.GetComponent<IEnemyController>();
            if (temp == null) return;
            if (temp.GetState() == State.Dash && CDBounce) return;
            HitSFX.Play();
            temp.PlayHitAnim();
            anim.Play("OnHit");
            CDBounce = true;
            StartCoroutine(CDDashBounce());
            Vector2 Dir = Vector2.down.normalized;
            Bounce(col,Dir);
            col.GetComponent<EnemyDashController>().AddHit();
        }
    }

    IEnumerator CDDashBounce()
    {
        yield return ScriptsTools.GetWait(0.2f);
        CDBounce = false;
    }
    

    private void OnCollisionEnter2D(Collision2D col)
    {
        HitSFX.Play();
        anim.Play("OnHit");
        if (col.gameObject.CompareTag("Score"))
        {
            ScoreSFX.Play();
            GameManager.current.AddScore();
            ActSpeed = (ActSpeed* SpeedMultiplier) + SpeedAdditive;
        }
        else if (col.gameObject.CompareTag("GameOver"))
        {
            GameOverSFX.Play();
            GameManager.current.LoseGame();
            PlayerController.current.Lose();
            rb.velocity = Vector2.zero;
        }
    }

    public void Bounce(Collider2D col, Vector2 Dir)
    {
        
        rb.velocity = Dir.normalized * ActSpeed ;
        
    }
}
