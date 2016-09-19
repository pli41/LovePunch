using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [SerializeField]
    LightManager lightManager;
    [SerializeField]
    AudioClip assignThemesong;
    static AudioClip themesong;
    [SerializeField]
    AudioClip assignLoseSong;
    static AudioClip loseClip;
    public Prince prince;
    [SerializeField]
    GameObject BGM;

    public CameraBlock cameraBlock;
    public static AudioSource[] BGMaudioSources;//0 for Themesong, 1 for ambient wind

    public GameObject[] minions;
    public LevelManager levelManager;
    public enum gameState {BeforeGame, Intro, InGame, BetweenLevels, King, AfterGame };
    public static gameState state;
	public gameState stateTest;
    
    public static List<GameObject> existingMinions;

	// Use this for initialization
	void Start () {
		BGM = GameObject.FindGameObjectWithTag ("BGM");
        BGMaudioSources = BGM.GetComponents<AudioSource>();
		state = gameState.BeforeGame;
        themesong = assignThemesong;
        loseClip = assignLoseSong;
        existingMinions = new List<GameObject>();
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
			CheckLevel ();
        }
        else if (state == gameState.BetweenLevels)
        {

        }
        else if (state == gameState.AfterGame)
        {

        }
        else if (state == gameState.King)
        {
            cameraBlock.start = true;
        }
	}

    public void StartIntro()
    {
        state = gameState.Intro;
        cameraBlock.start = true;
    }

    public void StartGame()
    {
		Debug.Log ("Start game");
        state = gameState.InGame;
		lightManager.StartGameLight ();
        
    }

    public void StartTheme()
    {
        if (!BGMaudioSources[0].isPlaying)
        {
            AudioPlay.PlaySound(BGMaudioSources[0], themesong);
        }
    }

    public void CheckLevel()
    {
		if (state == gameState.InGame && existingMinions.Count <= 0 && levelManager.GetLevelGeneration())
        {
            levelManager.SetupNextLevel();
        }
    }

    public void Reset()
    {
		state = gameState.InGame;
		levelManager.RestartCurrentLevel ();
        GameObject[] currentMinions = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject gameObject in currentMinions)
        {
            Destroy(gameObject);
        }
		prince.Reset ();
		AudioPlay.PlaySound (BGMaudioSources[0], themesong);
		BGMaudioSources [0].loop = true;
    }

    void CheckPrinceHealth()
    {
		if (prince.CheckDeath() && state == gameState.InGame)
        {
            Lose();
        }
    }

    public void Lose()
    {
		lightManager.RestartGame();
		Debug.Log ("Lose");
		BGMaudioSources [0].loop = false;
        AudioPlay.PlaySound(BGMaudioSources[0], loseClip);
        state = gameState.AfterGame;
    }

    public gameState GetGameState()
    {
        return state;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
