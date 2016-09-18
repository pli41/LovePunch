using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class RayCastTest : MonoBehaviour
{

    SteamVR_Controller.Device device;
    Button startButton;
    bool drawRay;
    LineRenderer lineRender;

    void Start()
    {

    }

    void Update()
    {
        RaycastHit raycastHit;
        GameObject gameObject = null;
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, Mathf.Infinity, LayerMask.GetMask("UI"), QueryTriggerInteraction.Collide))
        {
            gameObject = raycastHit.collider.gameObject;
            if (gameObject.GetComponent<Button>())
            {
                Debug.Log(gameObject.name + " button pressed");
                gameObject.GetComponent<Button>().onClick.Invoke();
            }
            //Do sth. with the found GameObject here
        }

    }
}