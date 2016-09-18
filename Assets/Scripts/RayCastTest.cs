using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class RayCastTest : MonoBehaviour
{
	
    SteamVR_Controller.Device device;
	Hand hand;

    void Start()
    {
		hand = GetComponent<Hand> ();
    }

    void Update()
    {
		device = hand.GetDevice ();
        RaycastHit raycastHit;
        GameObject gameObject;
		Debug.DrawRay (transform.position, transform.forward);
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, Mathf.Infinity, LayerMask.GetMask("UI"), QueryTriggerInteraction.Collide))
        {
			Debug.Log ("Hit UI");
            gameObject = raycastHit.collider.gameObject;
			if (gameObject.GetComponent<Button>() && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger ))
            {
                Debug.Log(gameObject.name + " button pressed");
                gameObject.GetComponent<Button>().onClick.Invoke();
            }
            //Do sth. with the found GameObject here
        }

    }
}