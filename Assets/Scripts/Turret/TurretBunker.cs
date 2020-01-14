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
    private SceneDataRetainer dataRetainer;

    private float lastShootTime = 0f;

    private float damage;
    private float fireRate;
    private float range;

    private void Start()
    {
        dataRetainer = SceneDataRetainer.GetInstance();
        range = status.radius * dataRetainer.bunkerMultipliers[0];
        damage = status.damage * dataRetainer.bunkerMultipliers[1];
        fireRate = status.fireRate / dataRetainer.bunkerMultipliers[2];

        gameManager = LevelManager.GetInstance();
        anim = GetComponent<Animator>();

        rangeUI.transform.localScale = new Vector3(range, range, 1f);
        bunkerWaveUI.localScale = new Vector3(range, range, 1f);
    }

    private void Update()
    {
        if (!gameManager.gameIsPaused)
        {
            if (Time.time - lastShootTime > fireRate)
            {
                FireBunker();
            }
        }
    }

    void FireBunker()
    {
        GetTargets();

        if(enemiesInRangeIndex > 0)
        {
            lastShootTime = Time.time;
            anim.SetTrigger("PlayAnim");
        }
        for (int index = 0; index < enemiesInRangeIndex; index++)
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

            enemiesInRange[index].TakeDamage(damage);
        }
    }

    private void GetTargets()
    {
        enemiesInRangeIndex = 0;
        Enemy[] enemyInstances = GameObject.FindObjectsOfType<Enemy>();

        foreach(Enemy enemy in enemyInstances)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance <= range)
            {
                enemiesInRange[enemiesInRangeIndex] = enemy;
                enemiesInRangeIndex++;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && status.canSlow)
        {
            other.GetComponent<Enemy>().Slow();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy" && status.canSlow)
        {
            other.GetComponent<Enemy>().StopSlow();
        }
    }
}
