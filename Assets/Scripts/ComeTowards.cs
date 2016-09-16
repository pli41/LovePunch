using UnityEngine;
using System.Collections;

public class ComeTowards : MonoBehaviour {

    private Transform target;
    public float speed = 1.0f;

	// Use this for initialization
	void Start () {
		if (target == null) 
		{
			target = GameObject.FindWithTag ("Player").GetComponent<Transform> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
			Vector3 targetPoint = target.position;
			targetPoint.y = transform.position.y;
			transform.LookAt(targetPoint);

			Vector3 destPos = new Vector3(target.position.x, transform.position.y, target.position.z);
			transform.position = Vector3.MoveTowards(transform.position, destPos, speed * Time.deltaTime);
            
            Debug.Log(transform.position);
        }
    }
}
