using UnityEngine;
using System.Collections;

public class BomberMinion : Enemy {

    public Transform target;

    private bool inRange;
    private bool isBombDropped = false;
    
    [SerializeField]
    AudioClip[] punchSounds;
    
    AudioSource audioSource;
    

    [SerializeField]
    Transform bombChild;

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
            CalculateVelocity();
        }

    }

    void FindTarget()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").GetComponent<Transform>();
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
        if (col.gameObject.CompareTag("Player"))
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
        bombChild.gameObject.GetComponents<Collider>()[0].enabled = true;
        bombChild.gameObject.GetComponents<Collider>()[1].enabled = true;
    }

    //Out of Range
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            inRange = false;
            Invoke("Death", 3f);
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
