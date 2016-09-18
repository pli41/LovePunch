using UnityEngine;
using System.Collections;

public class BomberMinion : Enemy {

    public Transform target;

    private bool inRange;
    private bool isBombDropped = false;
    
    [SerializeField]
    AudioClip[] punchSounds;
    Transform bombChild;
    AudioSource audioSource;
    

    // Use this for initialization
    void Start()
    {
        bombChild = transform.Find("Bomb");
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
            CalculateVelocity();
        }

    }

    void FindTarget()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Prince").GetComponent<Transform>();
        }
    }

    public override void Act()
    {
        base.Act();
        ComeTowards();
    }

    public void ComeTowards()
    {
        if (!CheckDisabled())
        {
            if (isBombDropped == false)
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
				Debug.Log ("Go back");
                transform.LookAt(target);
                transform.Rotate(0, 180, 0);
				transform.Translate(Vector3.forward * Time.deltaTime * GetSpeed());
            }
        }
        else
        {
            RunDisableTimer();
        }
    }

    //InRange
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Prince"))
        {
            inRange = true;

            // Drop Bomb

            //If spear exists, drop bomb, attach a rigid body
            if (bombChild)
            {
                ThrowBomb();
            }

        }
    }

    void ThrowBomb()
    {
        isBombDropped = true;
        bombChild.SetParent(null);
        Rigidbody bombRigidBody = bombChild.gameObject.AddComponent<Rigidbody>();
        bombRigidBody.mass = 1;
        bombRigidBody.isKinematic = false;
        bombChild.gameObject.GetComponent<Bomb>().DetachFromEnemy();
    }

    //Out of Range
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Princess"))
        {
            inRange = false;
            Destroy(gameObject, 5f);
        }
    }

    public override void Death()
    {
        base.Death();
        ThrowBomb();
    }

   public override void ReceiveDamage(float damage)
    {
        base.ReceiveDamage(damage);
        ThrowBomb();
        Disable();
    }

    protected override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col);
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (CheckDisabled())
            {
                Enemy enemy = col.gameObject.GetComponent<Enemy>();
                if (!enemy.CheckDisabled())
                {
                    enemy.ReceiveDamage(GetCurrentVelocity() * GetThrowFactor());
                    AudioPlay.PlayRandomSound(audioSource, punchSounds);
                }
            }
        }

        //Normal Minions Dead hit ground

    }

    public override void DealDamage(GameObject victim)
    {
        victim.GetComponent<Prince>().ReceiveDamage(GetDamage(), false);
    }

}
