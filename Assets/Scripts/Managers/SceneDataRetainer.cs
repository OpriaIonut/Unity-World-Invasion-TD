using UnityEngine;

public class SceneDataRetainer : MonoBehaviour {

    #region Singleton
    public static SceneDataRetainer instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Error! more than one instance for the build manager in the scene.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public int permanentUpgradeMoney;
    public int moneyPerLevel = 200;
    public int moneyMultplier = 25;
    public int[] starsPerLevel = new int[30];

    public float[] cannonMultipliers = new float[3];
    public float[] bunkerMultipliers = new float[3];
    public float[] laserMultipliers = new float[3];

    [Header("0 - range; 1 - damage; 2 - fireRate")]
    //It can only hold values between 0 and 4 inclusive
    public int[] cannonCurrentStatusLevel = new int[3];
    public int[] bunkerCurrentStatusLevel = new int[3];
    public int[] laserCurrentStatusLevel = new int[3];
}
