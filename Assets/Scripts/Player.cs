using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField]
    float maxHP;

    public float hp;
    GameManager gameManager;

	// Use this for initialization
	void Start () {
        hp = maxHP;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ReceiveDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        gameManager.Lose();
    }

}
