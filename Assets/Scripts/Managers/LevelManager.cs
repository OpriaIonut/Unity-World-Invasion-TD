using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    //Singleton
    public static LevelManager instance;

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
    //End singleton

    public int currentLives = 20;
    public Text livesText;
    public bool gameIsPaused = false;
    public GameObject pauseGameCanvas;

    private void Start()
    {
        pauseGameCanvas.SetActive(gameIsPaused);
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
        LoadScene("MainMenu");
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
