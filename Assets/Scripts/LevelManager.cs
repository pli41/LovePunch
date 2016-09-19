using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    public GameManager gameManager;
    public MinionSpawner[] spawners;
    public Level[] levels = new Level[5];

    public Level currentLevel;
    Timer waveTimer;
    int currentWave = 0;
    bool creating;
    [SerializeField]
    float sameSpawnerDelayTime;
    bool levelGenerationDone;

    int[] pickedSpawners;

    [SerializeField]
    float levelIntervals;

    [SerializeField]
    AudioClip[] KingVoices;
    int KingVoiceIndex;
    [SerializeField]
    AudioClip[] PrinceVoices;
    int PrinceVoiceIndex;
    bool betweenLevelFinished;
    bool finishInvoking;
    bool princeSpoken;
    bool kingSpoken;

    // Use this for initialization
    void Start () {
        
        levels[0] = new Level(
            1,
            new int[][] {
                new int[] { 0, 0 },
				new int[] { 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0},
                new int[] { 1, 4, 4, 4, 1, 2, 2 }
            },
            new float[] { 10f, 20f, 20f, 30f}
        );

        levels[1] = new Level(
            2,
            new int[][] {
                new int[] { 1, 1, 1 },
                new int[] { 0, 0, 1, 1 },
                new int[] { 0, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 0, 0, 0, 0 }
            },
            new float[] { 10f, 20f, 20f, 20f }
        );

        levels[2] = new Level(
            3,
            new int[][] {
                new int[] { 1, 3, 0 },
                new int[] { 3, 0, 1, 1 },
                new int[] { 0, 1, 1, 1, 3 },
                new int[] { 1, 1, 1, 0, 0, 0, 3 }
            },
            new float[] { 10f, 25f, 25f, 25f }
        );

        levels[3] = new Level(
            4,
            new int[][] {
                new int[] { 0, 0, 0, 4 },
                new int[] { 0, 0, 1, 1, 1, 4 },
                new int[] { 4, 4, 0, 3, 1 },
                new int[] { 4, 4, 3, 3, 1, 0, 0 }
            },
            new float[] { 10f, 30f, 30f, 30f }
        );
        /*
        levels[1] = new Level(

        );

        levels[2] = new Level(

        );

        levels[3] = new Level(

        );

        levels[4] = new Level(

        );
        */
        currentLevel = levels[0];
        creating = false;

    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.state == GameManager.gameState.InGame)
        {
            
            if (betweenLevelFinished)
            {
                HandleSpawning();
            }
            else
            {
                HandleBetweenLevels();
            }
        }
    }

    void HandleBetweenLevels()
    {
        if (!finishInvoking)
        {
            Invoke("KingSpeaks", 5f);
            Invoke("PrinceSpeaks", 10f);
            finishInvoking = true;
        }

        if (kingSpoken && princeSpoken)
        {
            betweenLevelFinished = true;
            kingSpoken = false;
            princeSpoken = false;
        }
    }



    void KingSpeaks()
    {

        KingVoiceIndex++;
        kingSpoken = true;
    }

    void PrinceSpeaks()
    {
        PrinceVoiceIndex++;
        finishInvoking = false;
        princeSpoken = true;
    }

    void HandleSpawning()
    {
		if (!creating && !levelGenerationDone)
        {
            creating = true;
            waveTimer = new Timer(currentLevel.waveIntervals[currentWave], CreateWave, false);
        }
        else
        {
            waveTimer.RunTimer();
        }
    }
    /*
    void RunMinionTimers()
    {
        foreach (Timer timer in minionTimers)
        {
            timer.RunTimer();
        }
    }
    */

    void CreateWave()
    {
        pickedSpawners = new int[currentLevel.waves[currentWave].Length];
        //chooose random spawners
        for (int i = 0; i < pickedSpawners.Length; i++)
        {
            pickedSpawners[i] = Random.Range(0, spawners.Length-1);
        }
        int minionIndex = 0;
        foreach (int spawnerNum in pickedSpawners)
        {
            GameObject minion = gameManager.minions[currentLevel.waves[currentWave][minionIndex]];
            

            CreateMinion(spawners[spawnerNum].gameObject, minion);
			//Debug.Log ("Minion " + minion.name +"spawned at" + Time.time);
			minionIndex++;
        }

        if (currentWave < currentLevel.waves.Length-1)
        {
			currentWave++;
        }
        else
        {
            levelGenerationDone = true;
        }
        creating = false;
    }

    void CreateMinion(GameObject spawner, GameObject minion)
    {
        spawner.GetComponent<MinionSpawner>().Spawn(minion);
    }

    public void SetupNextLevel()
    {
        betweenLevelFinished = false;
        GameManager.state = GameManager.gameState.InGame;
        if (currentLevel.levelNum >= levels.Length)
        {
            currentLevel = levels[currentLevel.levelNum];
        }
        else
        {
            GameManager.state = GameManager.gameState.King;
        }

		ResetLevel ();
    }

	public void RestartCurrentLevel(){
		ResetLevel ();
	}

	void ResetLevel(){
		currentWave = 0;
		levelGenerationDone = false;
	}

    public class RandomLevel
    {
        public int levelNum;
        public int[] minions;
        public int minionNum;
        public float[] minionGenerationInterval;
        public float[] minionGenerationIncrease; //0 is increase step, 1 is increase interval
        public float[] minionPossibilities;

        public RandomLevel(int levelNum, int[] minions, int minionNum, float[] minionGenerationInterval, float[] minionGenerationIncrease, float[] minionPossibilities)
        {
            this.levelNum = levelNum;
            this.minions = minions;
            this.minionNum = minionNum;
            this.minionGenerationInterval = minionGenerationInterval;
            this.minionGenerationIncrease = minionGenerationIncrease; //0 is increase step, 1 is increase interval
            this.minionPossibilities = minionPossibilities;
        }

    }

    public class Level
    {
        public int levelNum;
        public int[][] waves;
        public float[] waveIntervals;

        public Level(int levelNum, int[][] waves, float[] waveIntervals)
        {
            this.levelNum = levelNum;
            this.waves = waves;
            this.waveIntervals = waveIntervals;
        }
    }

	public bool GetLevelGeneration(){
	
		return levelGenerationDone;
	}


}



/*
void FastenGeneration()
{
    generateTimeMin = generateTimeMin < 0 ? 0 : currentLevel.minionGenerationInterval[0] - currentLevel.minionGenerationIncrease[0];
    generateTimeMax = generateTimeMax < 0 ? 0 : currentLevel.minionGenerationInterval[1] - currentLevel.minionGenerationIncrease[0];
}

void HandleSpawning()
{
    if (!creating)
    {
        float generateTime = Random.Range(generateTimeMin, generateTimeMax);
        minionTimer = new Timer(generateTime, SpawnMinion, false);
        creating = true;
    }
    else
    {
        minionTimer.RunTimer();
    }
}

void SpawnMinion()
{
    float randomNum = Random.Range(0f, 1f);
    int minionNum = PickMinion(randomNum);

    Debug.Log("Minion Num" + minionNum);
    int spawnerNum = Random.Range(0, spawners.Length-1);
    spawners[spawnerNum].Spawn(gameManager.minions[minionNum]);
    creating = false;
}

int PickMinion(float randomNum)
{
    float probCheck = 0f;
    for (int i = 0; i < currentLevel.minions.Length; i++)
    {
        if (randomNum >= probCheck)
        {
            probCheck += currentLevel.minionPossibilities[i];
            if (randomNum <= probCheck)
            {
                return currentLevel.minions[i];
            }
        }
    }
    return 0;
}
*/
