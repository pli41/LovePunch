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
    bool isGrounded;
    bool targetIsPlayer;

    [SerializeField]
    float attackRange;

    // Use this for initialization
    void Start()
    {
        ragdollCtrl = GetComponent<RagdollTest>();
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
            if (Random.Range(0f, 1f) >= 0f)
            {
                target = gameManager.player.transform;
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
        animator.SetBool("ReadyToRun", false);
        if (!CheckDisabled())
        {
            if (target != null && !CheckDeath() && isGrounded)
            {
                bool checkValid = false;
                if (targetIsPlayer)
                {
                    if (!inRange)
                    {
                        checkValid = true;
                    }
                }
                else
                {
                    if (!pickedUp && !pickingUp)
                    {
                        checkValid = true;
                    }
                }

                if (checkValid)
                {
                    //Debug.Log(gameObject.name + " is coming towards");
                    
                    animator.SetBool("InAttackRange", false);
                    animator.SetBool("InThrowRange", false);
                    animator.SetBool("ReadyToRun", true);
                    Vector3 targetPoint = target.position;
                    targetPoint.y = transform.position.y;
                    transform.LookAt(targetPoint);
                    Vector3 destPos = targetPoint;
                    //transform.position = Vector3.MoveTowards(transform.position, destPos, GetSpeed() * Time.deltaTime);
                    
                }
            }
        }
        else
        {
			RunDisableTimer ();
        }
    }

    public bool CheckInRange()
    {
        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
        return inRange;
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
                if (target.parent == null)
                {
                    animator.SetBool("InThrowRange", true);
                    raisePrinceTimer.RunTimer();
                    transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
                }
                
            }

            if (pickedUp)
            {
                animator.SetBool("InThrowRange", true);
                tossTimer.RunTimer();
                transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
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

        if ( col.gameObject.CompareTag("Player") && targetIsPlayer)
        {
            inRange = true;
            attackTimer = new Timer(2f, DealDamage, col.gameObject, true);
            animator.SetBool("InAttackRange", true);
            
        }


        if (col.gameObject.CompareTag("Prince") && !targetIsPlayer)  // angie
        {
            col.gameObject.GetComponent<Prince>().SetTerrify(true);
            Debug.Log(col.gameObject.name);
            pickingUp = true;
            raisePrinceTimer = new Timer(2f, RaisePrince, col.gameObject, false);
            animator.SetBool("InThrowRange", true);
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
            col.gameObject.GetComponent<Prince>().SetTerrify(false);
            if (pickingUp)
            {
                pickingUp = false;
                raisePrinceTimer.Reset();
            }
        }

        if (col.gameObject.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
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
        animator.SetTrigger("AttackTrigger");
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
        Invoke("Toss", 1.5f);
        gameManager.Lose();
    }

    public void Toss()
    {
        target.SetParent(null);
        Rigidbody rigid = target.gameObject.GetComponent<Rigidbody>();
        rigid.isKinematic = false;
        rigid.useGravity = true;
        rigid.AddForce((transform.forward + transform.up) * 100f, ForceMode.Impulse);
    }
    

    public override void Disable()
    {
        base.Disable();
        ReleasePrince();
    }

    public void ReleasePrince()
    {
        //Debug.Log(gameObject.name + " Released");
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
