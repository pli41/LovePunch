using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityStandardAssets.ImageEffects;

public class Prince : MonoBehaviour {

    [SerializeField]
    private float health;
    private bool isDeath;

    [SerializeField]
    public Hand[] hands;

    [SerializeField]
    AudioClip[] hurtSounds;
    [SerializeField]
    AudioClip[] hurtByPlayerSounds;

	GameManager gameManager;


    // Use this for initialization
    void Start () {
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
	}

   
}
