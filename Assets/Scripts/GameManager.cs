using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField]
    AudioClip assignThemesong;
    static AudioClip themesong;
    [SerializeField]
    AudioClip assignLoseSong;
    static AudioClip loseClip;
    [SerializeField]
    public Prince prince;
    [SerializeField]
    GameObject BGM;
    public static AudioSource[] BGMaudioSources;//0 for Themesong, 1 for ambient wind

    public GameObject[] minions;
    public LevelManager levelManager;
    public enum gameState {BeforeGame, InGame, BetweenLevels, AfterGame };
    public static gameState state;
	public gameState stateTest;


	// Use this for initialization
	void Start () {
		BGM = GameObject.FindGameObjectWithTag ("BGM");
        BGMaudioSources = BGM.GetComponents<AudioSource>();
		state = gameState.BeforeGame;
        themesong = assignThemesong;
        loseClip = assignLoseSong;
	}
	
	// Update is called once per frame
	void Update () {
		stateTest = state;
        if (state == gameState.BeforeGame)
        {
            
        }
        else if(state == gameState.InGame)
        {
			CheckPrinceHealth();
        }
        else if (state == gameState.BetweenLevels)
        {

        }
        else if (state == gameState.AfterGame)
        {

        }
	}

    public static void StartGame()
    {
		Debug.Log ("Start game");
        state = gameState.InGame;
        if (!BGMaudioSources[0].isPlaying)
        {
            AudioPlay.PlaySound(BGMaudioSources[0], themesong);
        }
    }

    public void Reset()
    {
        state = gameState.BeforeGame;
        GameObject[] minions = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject gameObject in minions)
        {
            Destroy(gameObject);
        }
		prince.Reset ();
		BGMaudioSources [0].loop = true;
    }

    void CheckPrinceHealth()
    {
		if (prince.CheckDeath() && state == gameState.InGame)
        {
            Lose();
        }
    }

    public static void Lose()
    {
		Debug.Log ("Lose");
		BGMaudioSources [0].loop = false;
        AudioPlay.PlaySound(BGMaudioSources[0], loseClip);
        state = gameState.AfterGame;
    }

    public gameState GetGameState()
    {
        return state;
    }
}
