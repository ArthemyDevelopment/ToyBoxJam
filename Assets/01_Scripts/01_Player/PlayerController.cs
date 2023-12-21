
using System.Collections;
using UnityEngine;


public class PlayerController : SingletonManager<PlayerController>
{

    public GameObject HitBox;
    public Rigidbody2D HitBoxRb;
    public float timeHitBox;
    public float PositionX; 
    

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        
    }


    void CheckInput()
    {
#if UNITY_WEBGL

        
        var ScreenPoint = Input.mousePosition;
        ScreenPoint.z = 10;
        PositionX = Camera.main.ScreenToWorldPoint(ScreenPoint).x;
        
        transform.position = new Vector2(PositionX, transform.position.y);
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("HitBall");
            StartCoroutine(ActiveHitBox());
        }

#endif

#if UNITY_ANDROID
        var ScreenPoint = Input.GetTouch[0].position;
        ScreenPoint.z = 10;
        PositionX = Camera.main.ScreenToWorldPoint(ScreenPoint).x;
        
        transform.position = new Vector2(PositionX, transform.position.y);
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("HitBall");
            StartCoroutine(ActiveHitBox());
        }

#endif

    }

    IEnumerator ActiveHitBox()
    {
        HitBox.SetActive(true);
        //HitBoxRb.WakeUp();
        yield return ScriptsTools.GetWait(timeHitBox);
        HitBox.SetActive(false);
    }

}
