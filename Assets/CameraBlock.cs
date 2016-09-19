using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraBlock : MonoBehaviour {

    public bool start;
    MeshRenderer meshRend;
    [SerializeField]
    Transform VRCamera;
    [SerializeField]
    float forwardOffset;
    [SerializeField]
    Movie movie;

    GameManager gameManager;
    Animator animator;
    bool fadingToBlack;
    bool fadingBlackTo;

    public float transparency;
    public enum blockState { idle, fadingToBlack, Black, fadingBlackTo};
    public blockState currentState;
	// Use this for initialization
	void Start () {
        currentState = blockState.idle;
        if (!VRCamera)
        {
            VRCamera = GameObject.FindGameObjectWithTag("Player").transform;
        }
        meshRend = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }
	
	// Update is called once per frame
	void Update () {        
        meshRend.enabled = start;
        BlockCamera();

        if (start && currentState == blockState.idle)
        {
            currentState = blockState.fadingToBlack;
        }

        if (currentState == blockState.fadingToBlack)
        {
            HandleFadingToBlack();
        }
        else if (currentState == blockState.Black)
        {
            PlayMovie();
        }
        else if (currentState == blockState.fadingBlackTo)
        {
            HandleFadingBlackTo();
        }
	}

    public void HandleFadingToBlack()
    {
        if (!fadingToBlack)
        {
            fadingToBlack = true;
            animator.SetTrigger("FadeToBlack");
        }
        else
        {
            Color color = meshRend.material.color;
            meshRend.material.color = new Color(color.r, color.g, color.b, transparency);
            if (transparency == 1f )
            {
                currentState = blockState.Black;
            }
        }
    }

    public void HandleFadingBlackTo()
    {
        Debug.Log(234);
        if (!fadingBlackTo)
        {
            fadingBlackTo = true;
            animator.SetTrigger("FadeBlackTo");
        }
        else
        {
            Color color = meshRend.material.color;
            meshRend.material.color = new Color(color.r, color.g, color.b, transparency);
            if (transparency == 0)
            {
                Finish();
            }
        }
    }

    public void Finish()
    {
        currentState = blockState.idle;
        fadingBlackTo = false;
        fadingToBlack = false;
        start = false;
        gameManager.StartGame();
    }

    void PlayMovie()
    {
        BlockCamera();
        movie.Play();
        Debug.Log("123");
    }

    void BlockCamera()
    {
        transform.position = VRCamera.transform.position + VRCamera.transform.forward * forwardOffset;
        transform.LookAt(VRCamera.transform.position);
    }

}
