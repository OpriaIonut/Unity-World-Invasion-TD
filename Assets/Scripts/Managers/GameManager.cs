using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Singleton
    public static GameManager instance;

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

    private GameObject waveSpawner;

    private void Start()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>().gameObject;
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
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
