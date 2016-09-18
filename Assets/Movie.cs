using UnityEngine;
using System.Collections;

public class Movie : MonoBehaviour {

    MovieTexture movie;

    // Use this for initialization
    void Start () {
        Renderer r = GetComponent<Renderer>();
        movie = (MovieTexture)r.material.mainTexture;
        
    }
	
	public void Play()
    {
        movie.Play();
    }
}
