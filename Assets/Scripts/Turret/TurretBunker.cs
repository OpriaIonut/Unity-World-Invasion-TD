using UnityEngine;

public class TurretBunker : MonoBehaviour
{
    public TurretStatus status;
    public string turretName;       //Used to find what build menu to activate

    //Bunker variables
    private Animator anim;
    public Transform bunkerWaveUI;

    //Default variables
    public SpriteRenderer rangeUI;

    private Enemy[] enemiesInRange = new Enemy[10];
    private short enemiesInRangeIndex = 0;
    private LevelManager gameManager;

    private float lastShootTime = 0f;

    private void Start()
    {
        gameManager = LevelManager.instance;
        anim = GetComponent<Animator>();

        rangeUI.transform.localScale = new Vector3(status.radius, status.radius, 1f);
        bunkerWaveUI.localScale = new Vector3(status.radius, status.radius, 1f);
    }

    private void Update()
    {
        if (!gameManager.gameIsPaused)
        {
            if (Time.time - lastShootTime > status.fireRate)
            {
                FireBunker();
                lastShootTime = Time.time;
            }
        }
    }

    void FireBunker()
    {
        GetTargets();

        if(enemiesInRangeIndex > 0)
        {
            anim.SetTrigger("PlayAnim");
        }
        for (int index = 0; index < enemiesInRangeIndex; index++)
        {

            //Sometimes, if we kill an enemy, the script is still selected as null, so we need to check beforehand if we killed him or not
            if (enemiesInRange[index] != null)
            {
                if (status.canSlow)
                {
                    enemiesInRange[index].Slow();
                }
                if (status.canStun)
                {
                    float randomValue = Random.Range(0f, 100f);
                    if (randomValue <= status.probability)
                    {
                        enemiesInRange[index].Stun();
                    }
                }

                enemiesInRange[index].TakeDamage(status.damage);
            }
        }
    }

    private void GetTargets()
    {
        enemiesInRangeIndex = 0;
        Enemy[] enemyInstances = GameObject.FindObjectsOfType<Enemy>();

        foreach(Enemy enemy in enemyInstances)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance <= status.radius)
            {
                enemiesInRange[enemiesInRangeIndex] = enemy;
                enemiesInRangeIndex++;
            }
        }
    }
}
