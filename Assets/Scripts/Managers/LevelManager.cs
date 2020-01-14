using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    #region Singleton
    private static LevelManager instance;
    public static LevelManager GetInstance() { return instance; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Error! More than one game manager in scene!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    public int currentLevel = 1;
    public int currentLives = 20;
    public Text livesText;
    public bool gameIsPaused = false;
    public GameObject pauseGameCanvas;

    public Text moneyWonText;
    public GameObject gameWonUI;
    public GameObject[] levelStars;
    public GameObject gameOverUI;

    private SceneDataRetainer data;
    private bool gameWon = false;
    private bool allEnemiesSpawned = false;

    private float currentTime = 0f;

    private void Start()
    {
        data = SceneDataRetainer.GetInstance();
        pauseGameCanvas.SetActive(gameIsPaused);
    }

    private void Update()
    {
        if (Time.time - currentTime > 1f)
        {
            if (allEnemiesSpawned && gameWon == false)
            {
                Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

                if(enemies.Length == 0)
                {
                    gameWon = true;
                    Invoke("GameWon", 3f);
                }
            }
            currentTime = Time.time;
        }
    }

    private void GameWon()
    {
        float moneyDecimator = 2f;

        gameIsPaused = true;
        gameWonUI.SetActive(true);
        if (currentLives == 20 && data.starsPerLevel[currentLevel - 1] < 3)
        {
            data.starsPerLevel[currentLevel - 1] = 3;
            moneyDecimator = 1f;
        }
        else if (currentLives >= 10 && data.starsPerLevel[currentLevel - 1] < 2)
        {
            data.starsPerLevel[currentLevel - 1] = 2;
            moneyDecimator = 1f;
        }
        else if (data.starsPerLevel[currentLevel - 1] < 1)
        {
            data.starsPerLevel[currentLevel - 1] = 1;
            moneyDecimator = 1f;
        }

        for (int index = 0; index < 3; index++)
        {
            if (index < data.starsPerLevel[currentLevel - 1])
                levelStars[index].SetActive(true);
            else
                levelStars[index].SetActive(false);
        }

        int levelMoney = (int)(((data.moneyPerLevel + (currentLevel - 1) * data.moneyMultplier) * (data.starsPerLevel[currentLevel - 1] / 3f)) / moneyDecimator);
        int aux = levelMoney % 5;

        if (aux >= 3)
            levelMoney = levelMoney - aux + 5;
        else
            levelMoney = levelMoney - aux;

        moneyWonText.text = "Money won: $" + levelMoney;
        data.permanentUpgradeMoney += levelMoney;
        

        Invoke("LoadLevelSelect", 3f);
    }

    private void LoadLevelSelect()
    {
        LoadScene("LevelSelect");
    }

    public void AllEnemiesSpawned()
    {
        allEnemiesSpawned = true;
    }

    public void LoseLives(int ammount)
    {
        currentLives -= ammount;

        if(currentLives <= 0)
        {
            livesText.text = "0";
            GameOver();
        }

        livesText.text = "" + currentLives;
    }

    public void PauseGame()
    {
        gameIsPaused = !gameIsPaused;
        pauseGameCanvas.SetActive(gameIsPaused);
    }

    private void GameOver()
    {
        gameIsPaused = true;
        gameOverUI.SetActive(true);

        Invoke("LoadLevelSelect", 3f);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
