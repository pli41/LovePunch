using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("123123");
        HandleBloodStain(col);
    }

    void HandleBloodStain(Collider col)
    {
        if (col.gameObject.CompareTag("BloodStain"))
        {
            Rigidbody stainRigid = col.GetComponent<Rigidbody>();
            stainRigid.useGravity = false;
            //transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            stainRigid.velocity = Vector3.zero;
        }
    }
}
