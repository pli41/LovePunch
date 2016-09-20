using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraBlock : MonoBehaviour {

    public bool start;
    MeshRenderer meshRend;
    [SerializeField]
    Transform VRCamera;
    [SerializeField]
    float forwardOffset;
    [SerializeField]
    Movie movie1;
    [SerializeField]
    Movie movie2;

    GameManager gameManager;
    Animator animator;
    bool fadingToBlack;
    bool fadingBlackTo;
    public bool endingNext;

    public float transparency;
    public enum blockState { idle, fadingToBlack, Black, fadingBlackTo};
    public blockState currentState;
	// Use this for initialization
	void Start () {
        currentState = blockState.idle;
        if (!VRCamera)
        {
            VRCamera = GameObject.FindGameObjectWithTag("PlayerCamera").transform;
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
        if (start)
        {
            currentState = blockState.idle;
            fadingBlackTo = false;
            fadingToBlack = false;
            start = false;
            if (!endingNext)
            {
                endingNext = true;
                gameManager.StartGame();
            }
            else
            {
                SceneManager.LoadScene("Boss");
                
            }
        }

        
    }

    void PlayMovie()
    {
        BlockCamera();
        if (!endingNext)
        {
            movie1.Play();
        }
        else
        {
            movie2.Play();
        }
    }

    void BlockCamera()
    {
        transform.position = VRCamera.transform.position + VRCamera.transform.forward * forwardOffset;
        transform.LookAt(VRCamera.transform.position);
    }
}
