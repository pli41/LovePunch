using UnityEngine;
using System.Collections;

public class ArenaCheck : MonoBehaviour {

    [SerializeField]
    Hand[] hands;
    GameManager gameManager;
	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
	{
		//Debug.Log ("123");
		
        if (col.tag == "Enemy")
        {
			//Debug.Log ("enemey in the sky");
            if (hands[0] && hands[1])
            {
				//Debug.Log ("onfire!");
                hands[0].onFire = true;
                hands[1].onFire = true;
            }

            
        }
    }
}
