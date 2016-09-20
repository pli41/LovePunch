using UnityEngine;
using System.Collections;

public class UILookAt : MonoBehaviour {

    public Transform target;

	// Use this for initialization
	void Start () {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        //transform.LookAt(targetPos);


	}
}
