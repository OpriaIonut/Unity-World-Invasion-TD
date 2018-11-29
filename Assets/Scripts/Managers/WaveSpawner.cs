using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    public Transform spawnLocation;
    public Transform enemyParentTransform;
    public Transform healthbarParentTransform;

        [Header("Enemies Prefab")]
    public GameObject normalEnemy;
    public GameObject fastEnemy;
    public GameObject heavyEnemy;
    public GameObject normalHeavy;
    public GameObject fastHeavy;
    public GameObject superHeavy;
    public GameObject boss;
    public GameObject healthUI;
    [Space(10)]

        [Header("UI elements")]
    public Text waveText;
    public Text timerText;
    [Space(10)]

    public float startSpawningTime = 10f;
    public float timeBetweenEnemies = 1f;
    public float timeBetweenWaves = 3f;

    //This is a special variable, the string flux will dictate what to instantiate based on the following rules:
    //        first there will be a letter that will describe the type of enemy to instantiate:
    //        'n' for normal; 'f' for fast; 'h' for heavy; 's' for fast & heavy; 'a' for normal & heavy; 'b' for super heavy; 'B' for boss
    //        second, there will be a numberthat will describe the number of times to instantiate
    //         each string represents a wave
    public string[] waveDescriptor;

    private int enemyCount = 0;             //Count the number of enemies to spawn per set
    private int spawnIndex = 0;             //Index used to search waveDescriptor[waveIndex]
    private int waveIndex = 0;              //Index used to search each row of waveDescriptor
    private GameObject enemyToInstantiate;  //Auxiliar variable used to remember what enemy to instantiate
    private GameObject clone;               //Auxiliar variable used to remember the object that we instantiated, so we can change some of his status
    private HealthBar healthClone;          //Auxiliar variable used to remember the object that we instantiated, so we can change some of his status
    private GameManager gameManager;        //Used to know if game is paused or not
    private float textTimer;                //Timer used to change the timer on the screen

    private float startTimer = 0;               //Timer used to delay the start of the first wave
    private float spawnEnemiesTimer = 0;        //Timer used to delay the spawn time between enemies (used with timeBetweenEnemies)
    private float changeWaveTimer = 0;          //Timer used to delay the time between each wave (used with timeBetweenWaves)
    private float spawnEnemiesPausedTimer = 0;  //Timer that remembers the time that went by while the game was paused (used with spawnEnemiesTimer and timeBetweenEnemies)
    private float changeWavePausedTimer = 0;    //Timer that remembers the time that went by while the game was paused (used with changeWaveTimer and timeBetweenWaves)
    private bool reachedEndOfWave = true;       //Bool used to know if we spawned everything we needed to spawn for the current wave
    private bool reachedEndOfLevel = false;     //Bool used to stop spawning after we spawned all waves.

	void Start () {
        gameManager = GameManager.instance;
        waveText.text = "Wave\t0 / " + (waveDescriptor.Length - 1);
        textTimer = startSpawningTime;
	}

    private void Update()
    {
        //Show the timer on the screen
        if (gameManager.gameIsPaused == false)
        {
            textTimer -= Time.deltaTime;

            if (textTimer < 0)
                timerText.text = "0.00";
            else
                timerText.text = textTimer.ToString("F1");
        }

        //Remember the time that went by while the time was paused
        if (gameManager.gameIsPaused == true)
        {
            spawnEnemiesPausedTimer += Time.deltaTime;
            changeWavePausedTimer += Time.deltaTime;
        }


        if (reachedEndOfLevel == false && gameManager.gameIsPaused == false)
        {
            //If we are at the begining of the level, then we want to delay the spawn based on startSpawningTime
            if (startTimer == 0)
                startTimer = Time.time;

            if (Time.time - startTimer > startSpawningTime)
            {
                //Get the time it takes to complete the wave so we can show it on the screen
                //We only want to change it when it reaches 0 or we reach the end of the level
                if (waveIndex != waveDescriptor.Length - 1)
                {
                    if (textTimer <= 0)
                    {
                        int count = 0;
                        for (int index = 0; index < waveDescriptor[waveIndex].Length; index += 2)
                            count += waveDescriptor[waveIndex][index + 1] - '0';
                        textTimer = timeBetweenEnemies * count + timeBetweenWaves;
                    }
                }
                else textTimer = 0;

                //If we spawned all enemies in the set, then we want to get enemies from the next set
                if (enemyCount == 0 && reachedEndOfWave == true)
                {
                    //Each time we change spawnEnemiesTimer, we need to make the paused timer = 0, because otherwise the difference: 
                    //Time.time - (spawnEnemiesTimer + spawnEnemiesPausedTimer) would be negative, and it should never be negative
                    spawnEnemiesTimer = Time.time;
                    spawnEnemiesPausedTimer = 0;

                    if (spawnIndex < waveDescriptor[waveIndex].Length)
                    {
                        switch (waveDescriptor[waveIndex][spawnIndex])
                        {
                            case 'n':
                                enemyToInstantiate = normalEnemy;
                                break;
                            case 'f':
                                enemyToInstantiate = fastEnemy;
                                break;
                            case 'h':
                                enemyToInstantiate = heavyEnemy;
                                break;
                            case 'a':
                                enemyToInstantiate = normalHeavy;
                                break;
                            case 'b':
                                enemyToInstantiate = superHeavy;
                                break;
                            case 's':
                                enemyToInstantiate = fastHeavy;
                                break;
                            case 'B':
                                enemyToInstantiate = boss;
                                break;
                        }
                        enemyCount = 0;
                        for (int index = spawnIndex + 1; index < waveDescriptor[waveIndex].Length && waveDescriptor[waveIndex][index] >= '0' && waveDescriptor[waveIndex][index] <= '9'; index++){
                            enemyCount = enemyCount * 10 + waveDescriptor[waveIndex][index] - '0';
                        }
                    }
                }

                //If we need to spawn enemies
                if (enemyCount > 0)
                {
                    reachedEndOfWave = false;

                    //We check for the time, and if we can, we spawn an enemy
                    if (Time.time - (spawnEnemiesTimer + spawnEnemiesPausedTimer) > timeBetweenEnemies)
                    {
                        InstantiateClones();
                        enemyCount--;
                        spawnEnemiesTimer = Time.time;
                        spawnEnemiesPausedTimer = 0;

                        //If we spawned all enemies, we say that through the bool, and we increase the index, so that in the next update we may can pick the next set of enemies
                        if (enemyCount == 0)
                        {
                            reachedEndOfWave = true;
                            spawnIndex += 2;
                        }
                    }
                }

                //If we reached the end of the wave and we spawned everything
                if (spawnIndex == waveDescriptor[waveIndex].Length && reachedEndOfWave == true)
                {
                    //We wait for a while
                    if (changeWaveTimer == 0)
                    {
                        changeWaveTimer = Time.time;
                        changeWavePausedTimer = 0;
                    }
                    if (Time.time - (changeWaveTimer + changeWavePausedTimer) > timeBetweenWaves)
                    {
                        //If we didn't reach the end of the level
                        if (waveIndex < waveDescriptor.Length - 1)
                        {
                            //We bring almost everything back to the default state
                            spawnIndex = 0;
                            textTimer = 0;
                            changeWaveTimer = 0;
                            changeWavePausedTimer = 0;
                            waveIndex++;
                            waveText.text = "Wave\t" + waveIndex + " / " + (waveDescriptor.Length - 1);
                        }
                        else
                        {
                            //Else, we reached the end of the level
                            reachedEndOfLevel = true;
                        }
                    }

                }
            }
        }
    }

    private void InstantiateClones()
    {
        //Instantiate the enemy and the healthbar
        clone = Instantiate(enemyToInstantiate, enemyParentTransform);
        healthClone = Instantiate(healthUI, healthbarParentTransform).GetComponent<HealthBar>();
        //Place the enemy at the correct transform and make the connection between the healthbar and the enemy
        clone.transform.position = spawnLocation.transform.position;
        healthClone.targetScript = clone.GetComponent<Enemy>();
    }
}
