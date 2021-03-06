﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public RagdollTest ragdollCtrl;

    [SerializeField]
    new string name;
    [SerializeField]
    float damage;
    [SerializeField]
    float speed;
    [SerializeField]
    float health;
    bool isDead;
    public bool punchedDown;
    public float punchedDownBonus;
    bool untertaken;

    private Rigidbody[] _childrenRigidBodies;

    [SerializeField]
    GameObject healthBar;
    GameObject healthBarsObj;
	GameObject healthBarObj;

    bool disabled;
	Timer disableTimer;
	[SerializeField]
	float disabledTime;

    [SerializeField]
    Vector3 UIOffset;
	[SerializeField]
    Vector3 initialScale;

    GameObject player;

    [SerializeField]
    GameObject bloodAnimation;
    [SerializeField]
    GameObject bloodStain;

	[SerializeField]
	float throwFactor;

    [SerializeField]
    public float afterMass;

	Vector3 transformVelocity;
	Vector3 oldPos;

    // Use this for initialization
    void Start () {
        ragdollCtrl = GetComponent<RagdollTest>();
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
        
        healthBarsObj = GameObject.FindGameObjectWithTag("EnemyHealthBars");
        healthBarObj = (GameObject)Instantiate(healthBar, healthBarsObj.GetComponent<Transform>());
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

	public virtual void Disable()
	{
		//Debug.Log ("Disabled");
		disableTimer = new Timer(disabledTime, Recover, false);
		disabled = true;
	}

    public void Disable(float time)
    {
        //Debug.Log ("Disabled");
        disableTimer = new Timer(time, Recover, false);
        disabled = true;
    }

    void Recover()
	{
        punchedDown = false;
        disabled = false;
        if (transform.parent != null)
        {
            transform.parent.gameObject.GetComponent<Hand>().isSomethingPicked = false;
            gameObject.transform.SetParent(null);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            
        }
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
        if (punchedDown)
        {
            health -= damage * punchedDownBonus;
        }
        else
        {
            health -= damage;
        }
        CheckDeath();
    }

    public virtual void Act() {
		if (isDead && !untertaken)
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
		if (health <= 0)
        {
            isDead = true;
        }
        return isDead;
    }

    public bool killCheck(float comingDamage)
    {
        return (health - comingDamage) <= 0;
    }

    public virtual void Death()
    {
        //DisableColliders();
		untertaken = true;
        bloodAnimation.GetComponent<ParticleSystem>().collision.SetPlane(0, GameObject.FindGameObjectWithTag("Ground").transform);
        bloodAnimation.SetActive(true);
        //ragdollCtrl.ActivateRagdoll();


        Destroy (healthBarObj);
        Destroy(gameObject, 10f);
        GameManager.existingMinions.Remove(gameObject);
        
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground") && isDead && transformVelocity.magnitude <= 0.1f)
        {
			GameObject bloodStainObj = Instantiate(bloodStain, col.contacts[0].point, Quaternion.Euler(90f, 0f, 0f)) as GameObject;
			Destroy (bloodStainObj, 5f);
        }
    }

	public void DisableColliders(){
		Collider[] colliders = GetComponents<Collider> ();
		foreach (Collider collider in colliders) {
			collider.enabled = false;
		}
	}


	public float GetCurrentVelocity(){
		return transformVelocity.magnitude;	
	}

    public virtual void DealDamage(GameObject victim)
    {

    }

    public void SetUpAfterDeath(Vector3 punchVelocity, Vector3 point)
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().mass = afterMass;
        ragdollCtrl.ActivateRagdoll(punchVelocity, point, afterMass);
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
