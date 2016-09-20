using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RunMovement : MonoBehaviour {

    public float runSpeed = 3f;

    private Transform headObj;

    private bool isMovementPossible = true;

    public Text timerText;

    public float timeForBossLevel = 60f;

    private float timerCtr;

    private float prevHeadPosition;

    private Timer bossTimer;

    private Timer textUpdateTimer;
    // Use this for initialization
    void Start () {

        timerCtr = timeForBossLevel;

        headObj  =  gameObject.transform.Find("Camera (head)");
        Debug.Log(headObj);

        prevHeadPosition = headObj.transform.position.y;

        //bossTimer = new Timer(timeForBossLevel, GameOver, false);

        textUpdateTimer = new Timer(1f, updateTimerText, true);


    }

    void GameOver()
    {
        Debug.Log("GameOver");
        isMovementPossible = false;

        Renderer r = GameObject.FindWithTag("EndScreen").GetComponent<Renderer>();
        MovieTexture movie = (MovieTexture)r.material.mainTexture;
        movie.Play();
    }

    void updateTimerText()
    { 
        if(timerCtr>-1)
            timerText.text = (timerCtr--).ToString();
    }

    void onCollisionEnter(Collision col)
    {
        //Debug.Log("Collided");
        //if (col.gameObject.CompareTag("Enemy"))
        //{
        //    Debug.Log("Collided");
        //    isMovementPossible = false;
        //}
    }

    public void disableMovement()
    {
        isMovementPossible = false;
    }

    public void enableMovement()
    {
        isMovementPossible = true;
    }

    void onCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            isMovementPossible = true;
        }
    }

    void Update()
    {
        if (timerCtr > -1)
            textUpdateTimer.RunTimer();
        else {
            GameOver();
        }
    }

    void FixedUpdate () {

        if (isMovementPossible)
        {
            //Debug.Log(headObj.transform.position.y);
            float currentHeadPosition = headObj.transform.position.y;

            //Debug.Log(currentHeadPosition + " " + prevHeadPosition);

            float deltaYPosition = Mathf.Abs(currentHeadPosition - prevHeadPosition);

            //Debug.Log(deltaYPosition);

            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * (deltaYPosition * 300));

            prevHeadPosition = currentHeadPosition;
        }

    }

}
