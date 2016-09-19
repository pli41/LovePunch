using UnityEngine;
using System.Collections;

public class NormalMinion : Enemy {

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
    

    // Use this for initialization
    void Start()
    {
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

    public void TossPrince()
    {
        Debug.Log("prince is tossed");
        gameManager.Lose();
    }

    public override void Disable()
    {
        base.Disable();
        ReleasePrince();
    }

    public void ReleasePrince()
    {
        if (pickedUp)
        {
            Transform princeTransform = transform.Find("Prince");
            princeTransform.position -= raisedOffset;
            princeTransform.SetParent(null);
        }
        pickedUp = false;
        pickingUp = false;
    }
}
