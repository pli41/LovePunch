using UnityEngine;
using System.Collections;

public class NormalMinion : Enemy {

    public Transform target;
    private Timer raisePrinceTimer; // angie
    [SerializeField]
    private Vector3 raisedOffset; //angie

    private Timer attackTimer;

    private bool inRange;
    private bool pickedUp; //angie

    [SerializeField]
    float punchDisableTime;
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
            target = GameObject.FindWithTag("Prince").GetComponent<Transform>();
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
        /*
		if (!CheckDisabled()  && inRange && !CheckDeath())
        {
            attackTimer.RunTimer();
        }
        */
        if (pickedUp) // angie
        {
            raisePrinceTimer.RunTimer();
        }
    }

    public override void Death()
    {
        base.Death();
    }

    //InRange
    void OnTriggerEnter( Collider col)
    {
        /*
        if ( col.gameObject.CompareTag("Prince"))
        {
            inRange = true;
            attackTimer = new Timer(2f, DealDamage, col.gameObject, true);
        }
        */

        if (col.gameObject.CompareTag("Prince"))  // angie
        {
            Debug.Log(col.gameObject.name);
            pickedUp = true;
            raisePrinceTimer = new Timer(2f, RaisePrince, col.gameObject, false);
        }
    }

    private void RaisePrince(GameObject victim) // angie
    {
        if (victim != null)
        {
            victim.transform.position = transform.position;
            victim.transform.localRotation = Quaternion.Euler(-90f, 0f, 0.0f);
            victim.transform.position += raisedOffset;
            victim.transform.SetParent(gameObject.transform);
            Debug.Log("Prince is raised up!");
        }
    }

    //Out of Range
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Prince"))
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
        victim.GetComponent<Prince>().ReceiveDamage(GetDamage(),false   );
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
}
