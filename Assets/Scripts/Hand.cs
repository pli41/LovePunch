using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class Hand : MonoBehaviour
{
   
    int ControllerNum; // 0 for left, 1 for right;
    int lastPunchedNum;
    bool powerPunch;
    [SerializeField]
    float powerPunchMul;
    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    int lastPunchedMinionId;

    Rigidbody rigid;
    Vector3 transformVelocity;
    Vector3 oldPos;

    AudioSource audioSource;
    [SerializeField]
    AudioClip[] punchSounds;

    public bool onFire;
    public GameObject[] fireAnimations;
    public float onFireMultiNum;
    float fireMulti;

	private bool isEnemyPicked = false;

    void Awake()
    {
        //rigid = GetComponent<Rigidbody>();
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        oldPos = transform.position;
        audioSource = GetComponent<AudioSource>();
        SetControllerNum();
        lastPunchedMinionId = 0;
        fireMulti = onFireMultiNum;

        /*Component[] c = GameObject.FindWithTag ("Enemy").GetComponents<Component> ();

		for (int i = 0; i < c.Length; i++) 
		{
			Debug.Log (c[i].GetType ());
		}*/

    }

    void SetControllerNum()
    {
        if (gameObject.name == "Controller (right)")
        {
            ControllerNum = 1;
        }
        else
        {
            ControllerNum = 0;
        }
    }

    void HandleOnFire()
    {
        if (onFire)
        {
            foreach (GameObject fireAnimation in fireAnimations)
            {
                fireAnimation.SetActive(true);
            }
            fireMulti = onFireMultiNum;
        }
        else
        {
            foreach (GameObject fireAnimation in fireAnimations)
            {
                fireAnimation.SetActive(false);
            }
            fireMulti = 1f;
        }
    }

    void FixedUpdate()
    {
        //rigid.WakeUp();
        device = SteamVR_Controller.Input((int)trackedObj.index);
        //Debug.Log(device.velocity.magnitude);

        /*if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You are holding 'Touch' on the Trigger");
        }
*/
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You activated TouchDown on the Trigger");
        }

        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You activated TouchUp on the Trigger");
        }
        /*
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You are holding 'Press' on the Trigger");
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You activated PressDown on the Trigger");
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You activated PressUp on the Trigger");
        }
*/
    }


    void Update()
    {
		CalculateVelocity ();
        HandleOnFire();
        //GameObject mySphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //mySphere.transform.localScale = transform.localScale;
        //mySphere.transform.position = transform.position;




        //Debug.Log(transformVelocity);
    }

	public void CalculateVelocity(){
		transformVelocity = (transform.position - oldPos) / Time.deltaTime;
		oldPos = transform.position;
	}

	public bool GetEnemyPicked(){
		return isEnemyPicked;
	}


    
    void OnTriggerEnter (Collider col)
    {
        /*
        if (col.CompareTag("Enemy"))
        {
            Debug.Log(device.velocity.magnitude);

            if (device.velocity.magnitude > 0.8)
            {
                Debug.Log("PUNCH");
                //tossObject(col.attachedRigidbody);
                Punch(col.attachedRigidbody);
            }
        }
        */

        if (col.GetComponent<Prince>())
        {
            if (transformVelocity.magnitude > 5f)
            {
                col.GetComponent<Prince>().ReceiveDamage(20f, true);

            }
        }

        //Debug.Log("You have collided with " + col.name + " and activated OnTriggerStay");
        //if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        //{
        //    Debug.Log("You have collided with " + col.name + " while holding down Touch");
        //    col.attachedRigidbody.isKinematic = true;
        //    col.gameObject.transform.SetParent(gameObject.transform);
        //}
        //if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        //{
        //    Debug.Log("You have released Touch while colliding with " + col.name);
        //    col.gameObject.transform.SetParent(null);
        //    col.attachedRigidbody.isKinematic = false;

        //    tossObject(col.attachedRigidbody);
        //}
    }
    

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Prince"))
        {
			if (!isEnemyPicked) {
				if (device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
					Vector3 movePrincessToPoint = transform.position;
					movePrincessToPoint.y = col.gameObject.transform.position.y;
					col.gameObject.transform.position = movePrincessToPoint;
				}
			}
        }

        if (col.CompareTag("Enemy"))
        {
            String EnemyName = col.GetComponent<Enemy>().GetName();
            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) && EnemyName.CompareTo("Bulker")!= 0)
            {
				isEnemyPicked = true;
                col.attachedRigidbody.isKinematic = true;
                col.gameObject.transform.SetParent(gameObject.transform);
                col.gameObject.GetComponent<NormalMinion>().Disable();
            }

            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                Debug.Log("Dropped");
                col.gameObject.transform.SetParent(null);
                col.attachedRigidbody.isKinematic = false;

                col.gameObject.GetComponent<NormalMinion>().ReceiveDamage(transformVelocity.magnitude * 5f);
                Debug.Log(transformVelocity.magnitude);

                tossObject(col.attachedRigidbody);

				isEnemyPicked = false;
            }

        }
    }


    void OnCollisionEnter(Collision col)
    {
        GameObject hit = col.gameObject;
        if (hit.CompareTag("Enemy"))
        {
            if (col.contacts.Length > 0)
            {
                if (lastPunchedMinionId != 0 && lastPunchedMinionId == hit.GetInstanceID())
                {
                    Debug.Log("SAME MINION HIT");
                    transformVelocity *= powerPunchMul;
                }

                lastPunchedMinionId = hit.GetInstanceID();

                Punch(hit.GetComponent<Rigidbody>(), col.contacts[0].point);
            }
        }

        if (GameManager.state == GameManager.gameState.BeforeGame)
        {
            GameManager.StartGame();
        }
    }

    void OnCollisionStay(Collision col)
    {
        
    }

    void Punch(Rigidbody rigidBody, Vector3 point)
    {
        if (ControllerNum == lastPunchedNum)
        {
            transformVelocity *= powerPunchMul;
        }

        device.TriggerHapticPulse(3000);

        //SteamVR_Controller.Input (0).TriggerHapticPulse (3800);

        transformVelocity *= fireMulti;
        lastPunchedNum = ControllerNum;

        rigidBody.gameObject.GetComponent<Enemy>().SetAfterMass();
        rigidBody.AddForceAtPosition(transformVelocity, point, ForceMode.Impulse);
        AudioPlay.PlayRandomSound(audioSource, punchSounds);
        GameObject obj = rigidBody.gameObject;
        if (obj.CompareTag("Enemy"))
        {
            obj.GetComponent<Enemy>().ReceiveDamage(transformVelocity.magnitude * 5f);
        }
    }


    void tossObject(Rigidbody rigidBody)
    {
        Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
        if (origin != null)
        {
            rigidBody.velocity = origin.TransformVector(device.velocity * 2);
            rigidBody.angularVelocity = origin.TransformVector(device.angularVelocity);
        }
        else
        {
            rigidBody.velocity = device.velocity * 2;
            rigidBody.angularVelocity = device.angularVelocity;
        }
    }

}
