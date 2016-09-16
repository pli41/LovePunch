using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    public GameManager gameManager;
    public MinionSpawner[] spawners;
    public Level[] levels = new Level[5];

    public Level currentLevel;
    Timer minionTimer;
    Timer fastTimer;
    bool creating;
    float generateTimeMin;
    float generateTimeMax;

	// Use this for initialization
	void Start () {
        
        levels[0] = new Level(
            1,
            new int[]{0, 1, 2},
            100,
            new float[] { 6, 12},
            new float[] { 0.5f, 20f},
            new float[] { 0.5f, 0.3f, 0.2f}
        );

        levels[1] = new Level(
            2,
            new int[] { 1 },
            10,
            new float[] { 10, 15 },
            new float[] { 1, 10 },
            new float[] { 1 }
        );

        levels[2] = new Level(
            3,
            new int[] { 1 },
            10,
            new float[] { 10, 15 },
            new float[] { 1, 10 },
            new float[] { 1 }
        );

        levels[3] = new Level(
            4,
            new int[] { 1 },
            10,
            new float[] { 10, 15 },
            new float[] { 1, 10 },
            new float[] { 1 }
        );

        levels[4] = new Level(
            5,
            new int[] { 1 },
            10,
            new float[] { 10, 15 },
            new float[] { 1, 10 },
            new float[] { 1 }
        );
        currentLevel = levels[0];
        generateTimeMin = currentLevel.minionGenerationInterval[0];
        generateTimeMax = currentLevel.minionGenerationInterval[1];
        creating = false;
        fastTimer = new Timer(currentLevel.minionGenerationIncrease[1], FastenGeneration, true);
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.state == GameManager.gameState.InGame)
        {
            HandleSpawning();
            fastTimer.RunTimer();
        }
        
    }

    void FastenGeneration()
    {
        generateTimeMin = generateTimeMin < 0 ? 0 : generateTimeMin - currentLevel.minionGenerationIncrease[0];
        generateTimeMax = generateTimeMax < 0 ? 0 : generateTimeMax - currentLevel.minionGenerationIncrease[0];
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

    void NextLevel()
    {
       
    }

    public class Level
    {
        public int levelNum;
        public int[] minions;
        public int minionNum;
        public float[] minionGenerationInterval;
        public float[] minionGenerationIncrease; //0 is increase step, 1 is increase interval
        public float[] minionPossibilities;

        public Level(int levelNum, int[] minions, int minionNum, float[] minionGenerationInterval, float[] minionGenerationIncrease, float[] minionPossibilities)
        {
            this.levelNum = levelNum;
            this.minions = minions;
            this.minionNum = minionNum;
            this.minionGenerationInterval = minionGenerationInterval;
            this.minionGenerationIncrease = minionGenerationIncrease; //0 is increase step, 1 is increase interval
            this.minionPossibilities = minionPossibilities;
        }

    }



}
