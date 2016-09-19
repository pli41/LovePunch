using UnityEngine;
using System.Collections;

public class RagdollTest : MonoBehaviour
{

    private Rigidbody[] _childrenRigidBodies;
    private bool _getPunched;

    void Awake()
    {
        _childrenRigidBodies = this.GetComponentsInChildren<Rigidbody>();
        _getPunched = false;
    }

    // Use this for initialization
    void Start()
    {
        EnableRigidboiesKinematicInChildren(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))//if (_getPunched)
        {
            Debug.Log("get Punched. Ragdoll now");
            EnableRigidboiesKinematicInChildren(false);
            Destroy(this.GetComponent<Rigidbody>());
            this.GetComponent<Animator>().enabled = false;
        }
    }


    // Disable the rigidbodies of Children
    void EnableRigidboiesKinematicInChildren(bool _toKinematic)
    {
        if (_childrenRigidBodies != null)
        {
            foreach (Rigidbody _childRigidBody in _childrenRigidBodies)
            {
                _childRigidBody.isKinematic = _toKinematic;
            }
        }
    }
}
