using UnityEngine;
using System.Collections;

public class testCollision : MonoBehaviour {

    Rigidbody rigid;
    Vector3 transformVelocity;
    Vector3 oldPos;

	// Use this for initialization
	void Awake () {
        oldPos = transform.position;
        rigid = GetComponent<Rigidbody>();
    }
	
    void FixedUpdate()
    {
        //rigid.velocity = new Vector3(-3f, 0f, 0f);
        rigid.WakeUp();
    }

	// Update is called once per frame
	void Update () {
        transformVelocity = (transform.position - oldPos) / Time.deltaTime;
        oldPos = transform.position;
        //Debug.Log(velocity);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("collision with velocity: " + transformVelocity);
            col.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(transformVelocity, col.contacts[0].point, ForceMode.Impulse);
        }
        
    }

    void OnCollisionStay(Collision col)
    {
        Debug.Log("Staying");
    }
}
