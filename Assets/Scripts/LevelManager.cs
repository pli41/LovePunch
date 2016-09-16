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
    float generateTimeMin;
    float generateTimeMax;
    [SerializeField]
    float sameSpawnerDelayTime;

	// Use this for initialization
	void Start () {

        levels[0] = new Level(
            1,
            new int[][] {
                new int[] { 0, 0, 0 },
                new int[] { 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0 }
            },
            new float[] { 10f, 10f, 10f }
        );

        levels[1] = new Level(
            2,
            new int[][] {
                new int[] { 1, 3, 1 },
                new int[] { 1, 1, 3, 1 },
                new int[] { 1, 1, 4, 1, 4, 1, 1, 1 }
            },
            new float[] { 10f, 10f, 10f }
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
            HandleSpawning();
        }
    }

    void HandleSpawning()
    {
        if (!creating)
        {
            creating = true;
            waveTimer = new Timer(currentLevel.waveIntervals[currentWave], CreateWave, false);
        }
        else
        {
            waveTimer.RunTimer();
        }
    }

    void CreateWave()
    {
        int[] pickedSpawners = new int[currentLevel.waves[currentWave].Length];
        //chooose random spawners
        for (int i = 0; i < pickedSpawners.Length; i++)
        {
            pickedSpawners[i] = Random.Range(0, spawners.Length-1);
        }

        int lastSpawner = 0;
        int minionIndex = 0;
        foreach (int spawnerNum in pickedSpawners)
        {
            GameObject minion = gameManager.minions[currentLevel.waves[currentWave][minionIndex]];
            if (spawnerNum != lastSpawner)
            {
                CreateMinion(spawners[spawnerNum].gameObject, minion);
            }
            else
            {
                Timer delayTimer = new Timer(sameSpawnerDelayTime, CreateMinion, spawners[spawnerNum].gameObject, minion, false);
            }
        }
        if (++currentWave >= currentLevel.waves.Length)
        {
            SetupNextLevel();
        }
        
        creating = false;
    }

    void CreateMinion(GameObject spawner, GameObject minion)
    {
        spawner.GetComponent<MinionSpawner>().Spawn(minion);
    }

    void SetupNextLevel()
    {
        GameManager.state = GameManager.gameState.BetweenLevels;
        currentLevel = levels[currentLevel.levelNum];
        currentWave = 0;
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
