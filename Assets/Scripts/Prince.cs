﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityStandardAssets.ImageEffects;

public class Princess : MonoBehaviour, Damagable {

    [SerializeField]
    private float health;
    private bool isDeath;

    [SerializeField]
    public Hand[] hands;

    [SerializeField]
    public AudioClip[] hurtSounds;
    [SerializeField]
    public AudioClip[] hurtByPlayerSounds;

    // Use this for initialization
    void Start () {
        isDeath = false;

	}
	
	// Update is called once per frame
	void Update () {
        CheckDeath();
	}

    public void ReceiveDamage( float damage)
    {
        this.health -= damage;

        ColorCorrectionCurves worldColor = GameObject.FindWithTag("Player").GetComponent<ColorCorrectionCurves>();

		//Debug.Log (cc.saturation);

		//Debug.Log (worldColor==null);
        //worldColor.saturation = this.health / 100;
        
        foreach (Hand hand in hands)
        {
            hand.onFire = false;
        }
        AudioPlay.PlayRandomSound(GetComponent<AudioSource>(), hurtSounds);
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