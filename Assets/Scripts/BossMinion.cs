using UnityEngine;
using System.Collections;

public class BossMinion : Enemy
{

    public Transform target;
    private Timer raisePrinceTimer; // angie
    [SerializeField]
    private Vector3 raisedOffset; //angie

    private Timer tossTimer;

    private bool inRange;
    private bool pickedUp; //angie
    private bool pickingUp;

    [SerializeField]
    float punchDisableTime;
    [SerializeField]
    AudioClip[] punchSounds;
    GameManager gameManager;
    AudioSource audioSource;

    public GameObject explosionEffect;

    // Use this for initialization
    void Start()
    {
        FindTarget();
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        UpdateUI();
        if (GameManager.state == GameManager.gameState.InGame)
        {
            Act();

            CalculateVelocity();
        }

    }

    void FindTarget()
    {
        if (target == null)
        {
            //target = GameObject.FindWithTag("Prince").GetComponent<Transform>();
        }
    }

    public override void Act()
    {
        base.Act();
        ComeTowards();
        Attack();
    }

    public void ComeTowards()
    {
        if (!CheckDisabled())
        {
            if (target != null && !CheckDeath() && !pickedUp && !pickingUp)
            {
                Vector3 targetPoint = target.position;
                targetPoint.y = transform.position.y;
                transform.LookAt(targetPoint);
                Vector3 destPos = new Vector3(target.position.x, transform.position.y, target.position.z);
                transform.position = Vector3.MoveTowards(transform.position, destPos, GetSpeed() * Time.deltaTime);
            }
        }
        else
        {
            RunDisableTimer();
        }
    }

    public void Attack()
    {
        /*
		if (!CheckDisabled()  && inRange && !CheckDeath())
        {
            attackTimer.RunTimer();
        }
        */
        if (pickingUp) // angie
        {
            raisePrinceTimer.RunTimer();
        }

        if (pickedUp)
        {
            tossTimer.RunTimer();
        }
    }

    public override void Death()
    {
        base.Death();
    }

    //InRange
    void OnTriggerStay(Collider col)
    {
       
        /*
        if ( col.gameObject.CompareTag("Prince"))
        {
            inRange = true;
            attackTimer = new Timer(2f, DealDamage, col.gameObject, true);
        }
        */

        if (col.gameObject.CompareTag("Player"))  // angie
        {
            Debug.Log("Entry");
            col.gameObject.GetComponent<RunMovement>().disableMovement();

            //GameObject explosion = (GameObject)Instantiate(explosionEffect, transform.position, transform.rotation);
            //Destroy(gameObject, 1f);
            //Destroy(explosion, 5f);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))  // angie
        {
            Debug.Log("Exit");
            col.gameObject.GetComponent<RunMovement>().enableMovement();
        }
    }

    public override void DealDamage(GameObject victim)
    {
        victim.GetComponent<Prince>().ReceiveDamage(GetDamage(), false);
    }


    public override void ReceiveDamage(float damage)
    {
        base.ReceiveDamage(damage);
        if (name != "Bulker")
        {
            Disable();
        }
    }


    public override void Disable()
    {
        base.Disable();
    }

}
