using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, Damagable {

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
	float throwFactor;

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
		UpdateUI ();
		CalculateVelocity ();
    }

    public void SetupUI()
    {
        player = GameObject.FindGameObjectWithTag("Player");
		healthBarObj.transform.position = transform.position + UIOffset;
		initialScale = healthBarObj.transform.localScale;
        healthBarsObj = GameObject.FindGameObjectWithTag("EnemyHealthBars");
        healthBarObj = (GameObject)GameObject.Instantiate(healthBarObj, healthBarsObj.GetComponent<Transform>());
    }

    public void UpdateUI()
    {
        healthBarObj.GetComponent<Slider>().value = health;
        healthBarObj.transform.position = transform.position + UIOffset;
        healthBarObj.transform.LookAt(new Vector3(player.transform.position.x, healthBarObj.transform.position.y, player.transform.position.z));
        healthBarObj.GetComponent<RectTransform>().localScale = initialScale;
    }

	public void Disable()
	{
		Debug.Log ("Disabled");
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

    public virtual void Act() {}

    public bool CheckDeath()
    {
        if (GetHealth() <= 0)
        {
            Death();
        }
        return isDead;
    }

    public void Death()
    {
        isDead = true;
        //EnableRagdoll();
        //DisableColliders();
        bloodAnimation.SetActive(true);
		Destroy (healthBarObj, 10f);
        Destroy(gameObject, 10f);
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
}
