using UnityEngine;
using System.Collections;

public class RagdollTest : MonoBehaviour
{

    private Rigidbody[] _childrenRigidBodies;
    private Collider[] _childrenColliders;
    private bool _getPunched;

    void Awake()
    {
        _childrenRigidBodies = this.GetComponentsInChildren<Rigidbody>();
        _childrenColliders = this.GetComponentsInChildren<Collider>();
        _getPunched = false;
    }

    // Use this for initialization
    void Start()
    {
        EnableRigidboiesKinematicInChildren(true);
        EnableIsDynamicsInThis(false);
        EnableCollidersInChildren(false);
        EnableCollidersInThis(true);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateRagdoll()
    {
        Debug.Log("get Punched. Ragdoll now");
        EnableRigidboiesKinematicInChildren(false);

        EnableCollidersInChildren(true);
        EnableCollidersInThis(false);
        Destroy(this.GetComponent<Rigidbody>());
        this.GetComponent<Animator>().enabled = false;
    }

    public void ActivateRagdoll(Vector3 punchVelocity, Vector3 point)
    {
        this.ActivateRagdoll();
        float smallestDistance = 99999f;
        Rigidbody rigidPicked = _childrenRigidBodies[0];
        foreach (Rigidbody rigid in _childrenRigidBodies)
        {
            float dist = Vector3.Distance(rigid.transform.position, point);
            if (dist < smallestDistance)
            {
                rigidPicked = rigid;
            }
        }
        rigidPicked.AddForceAtPosition(punchVelocity * 20f, point, ForceMode.Impulse);

    }

    // Disable the rigidbodies of Children
    void EnableRigidboiesKinematicInChildren(bool _toKinematic)
    {
        if (_childrenRigidBodies != null)
        {

            foreach (Rigidbody _childRigidBody in _childrenRigidBodies)
            {

                if (_childRigidBody.gameObject.name != gameObject.name)
                {
                    _childRigidBody.isKinematic = _toKinematic;
                }
            }
        }
    }

    void EnableIsDynamicsInThis(bool state)
    {
        GetComponent<Rigidbody>().isKinematic = state;
    }

    void EnableCollidersInThis(bool state)
    {
        foreach (Collider col in GetComponents<Collider>())
        {
            col.enabled = state;
        }
    }

    void EnableCollidersInChildren(bool boolean)
    {
        if (_childrenColliders != null)
        {
            foreach (Collider _collider in _childrenColliders)
            {
                if (_collider.gameObject.name != gameObject.name)
                {
                    _collider.enabled = boolean;
                }
            }
        }
    }
}