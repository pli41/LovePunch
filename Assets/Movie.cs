using UnityEngine;
using System.Collections;

public class Movie : MonoBehaviour {

    CameraBlock cameraBlock;
    MovieTexture movie;
	Renderer r;
	[SerializeField]
	float movieLength;
	[SerializeField]
	Light movieLight;
	bool isPlaying;
	AudioSource audioSource;

    // Use this for initialization
    void Start () {
        r = GetComponent<Renderer>();
		Debug.Log (r.material.mainTexture);
        movie = (MovieTexture)r.material.mainTexture;
		movieLength = movie.duration;
		//r.enabled = false;
		audioSource = GetComponent<AudioSource>();
        cameraBlock = transform.parent.gameObject.GetComponent<CameraBlock>();
    }
	
	public void Play()
    {
		if(!isPlaying){
			isPlaying = true;
			r.enabled = true;
			movieLight.enabled = true;
			movie.Play();
			audioSource.clip = movie.audioClip;
			audioSource.Play ();
			Invoke ("Finish", movieLength);
		}
    }

	public void Finish(){
		Debug.Log ("Finish");
		audioSource.Stop ();
		movieLight.enabled = false;
        cameraBlock.currentState = CameraBlock.blockState.fadingBlackTo;
    }
}
