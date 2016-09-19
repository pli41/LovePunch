using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityStandardAssets.ImageEffects;

public class Prince : MonoBehaviour {

    [SerializeField]
    private float health;
    private bool isDeath;

    Vector3 originalPos;
    Quaternion originalRot;

    [SerializeField]
    public Hand[] hands;

    [SerializeField]
    AudioClip[] hurtSounds;
    [SerializeField]
    AudioClip[] hurtByPlayerSounds;

	GameManager gameManager;
    Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        originalPos = transform.position;
        originalRot = transform.rotation;
        isDeath = false;
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ReceiveDamage( float damage, bool friendly)
    {
        this.health -= damage;

        //ColorCorrectionCurves worldColor = GameObject.FindWithTag("Player").GetComponent<ColorCorrectionCurves>();

		//Debug.Log (cc.saturation);

		//Debug.Log (worldColor==null);
        //worldColor.saturation = this.health / 100;
		if(health <= 0f){
			isDeath = true;
			gameManager.Lose ();
		}

        foreach (Hand hand in hands)
        {
            hand.onFire = false;
        }
        if (friendly)
        {
			AudioPlay.PlayRandomSound(GetComponent<AudioSource>(), hurtByPlayerSounds);
        }
        else
        {
            AudioPlay.PlayRandomSound(GetComponent<AudioSource>(), hurtSounds);
        }
        
    }

    public bool CheckDeath()
    {
        if ( this.GetHealth() <= 0)
        {
			//isDeath = true;
        }
        return isDeath;
    }

    public float GetHealth ()
    {
        return health;
    }

	public void Reset(){
		health = 100f;
		isDeath = false;
		foreach (Hand hand in hands)
		{
			hand.onFire = false;
		}
        transform.position = originalPos;
        transform.rotation = originalRot;
	}

    public void ActivateRagdoll()
    {
        GetComponent<RagdollTest>().ActivateRagdoll();
        animator.SetBool("terrified", false);
    }

    public void DeactivateRagdoll()
    {
        GetComponent<RagdollTest>().DeactivateRagdoll();
    }

    public void SetTerrify(bool state)
    {
        animator.SetBool("terrified", state);
    }
}
