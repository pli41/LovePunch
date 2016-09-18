﻿using UnityEngine;
using System.Collections;

public class LightManager : MonoBehaviour {

    [SerializeField]
    GameObject[] fires;

    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void StartGameLight()
    {
        animator.SetTrigger("GameStart");
    }

    public void RestartGame()
    {
        animator.SetTrigger("Restart");
    }
}
