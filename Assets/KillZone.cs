using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<Enemy>())
        {
            col.gameObject.GetComponent<Enemy>().ReceiveDamage(9999f);
        }
        else
        {
            Debug.Log("WOW");
        }
    }
}
