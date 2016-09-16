using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public float score;
    private Slider healthSlider;


    [SerializeField] 
    private Text scoreText;
    
    // Use this for initialization
    void Start () {
        score = 0f;
        
	}
	
	void Update () {
        ShowScore();
        //ShowHealth();
    }

    public float GetScore()
    {
        return score;
    }

    private void ShowScore()
    {
        float currScore;
        currScore = GetScore();
        scoreText.text = "Score: " + currScore.ToString();// Update Score
    }

    private void ShowHealth( GameObject palyer)
    {
        //.Player.GetHealth() = 
    }

}
