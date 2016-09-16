using UnityEngine;
using System.Collections;

public class NormalMinion : Enemy {

    public Transform target;

    
    private Timer attackTimer;

    private bool inRange;

	[SerializeField]
	AudioClip[] punchSounds;

    AudioSource audioSource;
    

    // Use this for initialization
    void Start()
    {
        FindTarget();
        audioSource = GetComponent<AudioSource>();
        SetupUI();
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
            target = GameObject.FindWithTag("Princess").GetComponent<Transform>();
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
            if (target != null && !CheckDeath() && !inRange)
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
			RunDisableTimer ();
        }
    }

    public void Attack()
    {
		if (!CheckDisabled()  && inRange && !CheckDeath())
        {
            attackTimer.RunTimer();
        }       
    }
    
    //InRange
    void OnTriggerEnter( Collider col)
    {
        if ( col.gameObject.CompareTag("Princess"))
        {
            inRange = true;
            attackTimer = new Timer(2f, DealDamage, col.gameObject, true);
        }
    }

    //Out of Range
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Princess"))
        {
            inRange = false;
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
        victim.GetComponent<Prince>().ReceiveDamage(GetDamage(), true);
    }


    public override void ReceiveDamage(float damage)
    {
        base.ReceiveDamage(damage);
		Disable ();
    }
}
