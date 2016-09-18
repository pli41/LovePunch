using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraBlock : MonoBehaviour {

    public bool state;
    MeshRenderer meshRend;
    [SerializeField]
    Transform VRCamera;
    [SerializeField]
    float forwardOffset;
    [SerializeField]
    Movie movie;

	// Use this for initialization
	void Start () {
        if (!VRCamera)
        {
            VRCamera = GameObject.FindGameObjectWithTag("Player").transform;
        }
        meshRend = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        meshRend.enabled = state;
        if (state)
        {
            movie.Play();
            BlockCamera();
        }
	}

    void BlockCamera()
    {
        transform.position = VRCamera.transform.position + VRCamera.transform.forward * forwardOffset;
        transform.LookAt(VRCamera.transform.position);
    }
}
