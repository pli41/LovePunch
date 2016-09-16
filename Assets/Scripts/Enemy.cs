using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    [SerializeField]
    string name;
    [SerializeField]
    float damage;
    [SerializeField]
    float speed;
    [SerializeField]
    float health;
    bool isDead;


    private Rigidbody[] _childrenRigidBodies;

    [SerializeField]
    GameObject healthBarObj;
    GameObject healthBarsObj;
    

    bool disabled;
	Timer disableTimer;
	[SerializeField]
	float disabledTime;

    [SerializeField]
    Vector3 UIOffset;
    Vector3 initialScale;

    GameObject player;

    [SerializeField]
    GameObject bloodAnimation;
    [SerializeField]
    GameObject bloodStain;

	[SerializeField]
	float throwFactor;

    [SerializeField]
    float afterMass;

	Vector3 transformVelocity;
	Vector3 oldPos;

    // Use this for initialization
    void Start () {
        isDead = false;
		disabled = false;
        _childrenRigidBodies = this.GetComponentsInChildren<Rigidbody>();
        SetupUI();
    }

	// Update is called once per frame
	void Update () {
		Act ();
        UpdateUI();		
		CalculateVelocity ();
    }

    public void SetupUI()
    {
        if(healthBarObj)
            initialScale = healthBarObj.transform.localScale;
        healthBarsObj = GameObject.FindGameObjectWithTag("EnemyHealthBars");
        healthBarObj = (GameObject)Instantiate(healthBarObj, healthBarsObj.GetComponent<Transform>());
        player = GameObject.FindGameObjectWithTag("Player");
		healthBarObj.transform.position = transform.position + UIOffset;   
        healthBarObj.GetComponent<Slider>().maxValue = health;
    }

    public void UpdateUI()
    {
        if (healthBarObj)
        {
            healthBarObj.GetComponent<Slider>().value = health;
            healthBarObj.transform.position = transform.position + UIOffset;
            healthBarObj.transform.LookAt(new Vector3(player.transform.position.x, healthBarObj.transform.position.y, player.transform.position.z));
            healthBarObj.GetComponent<RectTransform>().localScale = initialScale;
        }
    }

	public void Disable()
	{
		//Debug.Log ("Disabled");
		disableTimer = new Timer(disabledTime, Recover, false);
		disabled = true;
	}

	void Recover()
	{
		disabled = false;
	}

	public float GetThrowFactor(){
		return throwFactor;
	}


	public void CalculateVelocity(){
		transformVelocity = (transform.position - oldPos) / Time.deltaTime;
		oldPos = transform.position;
	}

	public bool CheckDisabled(){
		return disabled;	
	}

	public void RunDisableTimer(){
		disableTimer.RunTimer ();
	}

    public virtual void ReceiveDamage(float damage) {
        health -= damage;
        CheckDeath();
    }

    public virtual void Act() {
        if (isDead)
        {
            Death();
        }

        if (GameManager.state != GameManager.gameState.InGame)
        {
            Disable();
        }
    }

    public bool CheckDeath()
    {
        if (GetHealth() <= 0)
        {
            isDead = true;
        }
        return isDead;
    }

    public void Death()
    {
        //EnableRagdoll();
        //DisableColliders();
        bloodAnimation.SetActive(true);
		Destroy (healthBarObj);
        
        Destroy(gameObject, 10f);
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground") && isDead && transformVelocity.magnitude <= 0.1f)
        {
            Instantiate(bloodStain, col.contacts[0].point, Quaternion.Euler(90f, 0f, 0f));
        }
    }

	public void DisableColliders(){
		Collider[] colliders = GetComponents<Collider> ();
		foreach (Collider collider in colliders) {
			collider.enabled = false;
		}
	}

    public void EnableRagdoll()
    {
        if (_childrenRigidBodies != null)
        {
            foreach (Rigidbody _childRigidBody in _childrenRigidBodies)
            {
                _childRigidBody.isKinematic = true;
            }          
        }
        Destroy(this.GetComponent<Rigidbody>());
        this.GetComponent<Animator>().enabled = false;
    }

	public float GetCurrentVelocity(){
		return transformVelocity.magnitude;	
	}

    public void DisableRagdoll()
    {
        if (_childrenRigidBodies != null)
        {
            foreach (Rigidbody _childRigidBody in _childrenRigidBodies)
            {
                _childRigidBody.isKinematic = false;
            }
            this.GetComponent<Animator>().enabled = true;
        }
    }

    public virtual void DealDamage(GameObject victim)
    {

    }

    public void SetAfterMass()
    {
        GetComponent<Rigidbody>().mass = afterMass;
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public float GetDamage()
    {
        return this.damage;
    }

    public string GetName()
    {
        return name;
    }
}
