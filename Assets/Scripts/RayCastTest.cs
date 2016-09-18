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

        device = transform.GetComponent<Hand>().GetDevice();
        startButton = GameObject.FindWithTag("StartButton").GetComponent<Button>();
        drawRay = true;
        lineRender = GetComponent<LineRenderer>();
        lineRender.useWorldSpace = true;
    }

    void Update()
    {
        RaycastHit hit;
        lineRender.enabled = drawRay;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.transform.gameObject.tag == "StartButton" && device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) // 
            {
                Debug.Log("Button Pressed by raycast");
                startButton.onClick.Invoke(); // invoke functions that will run after the button is pressed.
                drawRay = false;
                lineRender.enabled = drawRay;
            }
        }

        Vector3[] lineVertixes = new Vector3[2] { transform.position, hit.point };

        if (drawRay)
        {
            // lineRender.SetColors(Color.blue, Color.white);
            lineRender.SetPositions(lineVertixes);
        }
    }




}