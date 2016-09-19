using UnityEngine;
using System.Collections;

public class NormalMinion : Enemy {

    public Transform target;
    private Timer raisePrinceTimer; // angie
    [SerializeField]
    private Vector3 raisedOffset; //angie
    private Timer attackTimer;
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
    Animator animator;

    bool targetIsPlayer;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        FindTarget();
        audioSource = GetComponent<AudioSource>();
        SetupUI();
        tossTimer = new Timer(5f, TossPrince, false);
    }


    void Update()
    {
		UpdateUI();
        if (GameManager.state == GameManager.gameState.InGame)
        {
            Act();
			CalculateVelocity ();
        }
    }

    void FindTarget(){
        if (target == null)
        {
            if (Random.Range(0f, 1f) <= 0.5f)
            {
                target = GameObject.FindWithTag("Player").GetComponent<Transform>();
                targetIsPlayer = true;
            }
            else
            {
                target = GameObject.FindWithTag("Prince").GetComponent<Transform>();
                targetIsPlayer = false;
            }
            
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
                animator.SetBool("InAttackRange", false);
                Vector3 targetPoint = target.position;
                targetPoint.y = transform.position.y;
                transform.LookAt(targetPoint);
                Vector3 destPos = targetPoint;
                transform.position = Vector3.MoveTowards(transform.position, destPos, GetSpeed() * Time.deltaTime);
            }
        }
        else
        {
			RunDisableTimer ();
        }
    }

    public void Attack()
    {
        /*
		
        */
        if (targetIsPlayer)
        {
            if (!CheckDisabled() && inRange && !CheckDeath())
            {
                attackTimer.RunTimer();
            }
        }
        else
        {
            if (pickingUp) // angie
            {
                animator.SetBool("InAttackRange", true);
                raisePrinceTimer.RunTimer();
            }

            if (pickedUp)
            {
                animator.SetBool("InAttackRange", true);
                tossTimer.RunTimer();
            }
        }
    }

    public override void Death()
    {
        base.Death();
    }

    //InRange
    void OnTriggerEnter( Collider col)
    {

        if ( col.gameObject.CompareTag("Player"))
        {
            inRange = true;
            attackTimer = new Timer(2f, DealDamage, col.gameObject, true);
        }


        if (col.gameObject.CompareTag("Prince"))  // angie
        {
            Debug.Log(col.gameObject.name);
            pickingUp = true;
            raisePrinceTimer = new Timer(2f, RaisePrince, col.gameObject, false);
        }
    }

    private void RaisePrince(GameObject victim) // angie
    {
        if (victim != null)
        {
            victim.transform.position = transform.position;
            //victim.transform.localRotation = Quaternion.Euler(-90f, 0f, 0.0f);
            victim.transform.position += raisedOffset;
            victim.transform.SetParent(gameObject.transform);
            pickedUp = true;
            Debug.Log("Prince is raised up!");
        }
    }

    //Out of Range
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Prince"))
        {
            inRange = false;
            if (pickingUp)
            {
                pickingUp = false;
                raisePrinceTimer.Reset();
            }
        }
    }

	protected override void OnCollisionEnter(Collision col)
	{
        base.OnCollisionEnter(col);
		if (col.gameObject.CompareTag ("Enemy")) 
		{
			if (CheckDisabled()) {
				Enemy enemy = col.gameObject.GetComponent<Enemy> ();
				if (!enemy.CheckDisabled ()){
					enemy.ReceiveDamage (GetCurrentVelocity()*GetThrowFactor());
					AudioPlay.PlayRandomSound(audioSource, punchSounds);
				}
			}
		}

        //Normal Minions Dead hit ground

	}

    public override void DealDamage(GameObject victim)
    {
        victim.GetComponent<Player>().ReceiveDamage(GetDamage());
    }


    public override void ReceiveDamage(float damage)
    {
        base.ReceiveDamage(damage);
        if (name != "Bulker")
        {
            Disable();
        }
    }

    public void PunchedDown()
    {
        Disable(punchDisableTime);
        punchedDown = true;
    }

    public void TossPrince()
    {
        Debug.Log("prince is tossed");
        animator.SetTrigger("ThrowTrigger");
        gameManager.Lose();
    }

    public override void Disable()
    {
        base.Disable();
        ReleasePrince();
    }

    public void ReleasePrince()
    {
        Debug.Log(gameObject.name + " Released");
        if (pickedUp || pickingUp)
        {
            
            
            if (pickedUp)
            {
                Transform princeTransform = transform.Find("Prince");
                princeTransform.position -= raisedOffset;
                princeTransform.SetParent(null);
                tossTimer.Reset();
            }
            raisePrinceTimer.Reset();
            pickedUp = false;
            pickingUp = false;
        }
    }
}
