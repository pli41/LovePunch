using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour {

    bool isDisabled = false;

    GameObject playerObj;
    // Use this for initialization
    void Start () {

        playerObj = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (playerObj.transform.position.z > gameObject.transform.position.z + 30)
        {
            Destroy(gameObject, 2f);
        }
        else
        {
            //gameObject.transform.Translate(Vector3.forward * Time.deltaTime * Random.Range(1, 5));
        }
        
	}
}
