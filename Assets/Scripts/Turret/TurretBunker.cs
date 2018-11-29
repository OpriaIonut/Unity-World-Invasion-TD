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

    private Enemy[] enemiesInRange = new Enemy[15];
    private short enemiesInRangeIndex = 0;
    private GameManager gameManager;

    private float lastShootTime = 0f;
    private float lastChangeTargetTime = 0f;

    private void Start()
    {
        gameManager = GameManager.instance;
        anim = GetComponent<Animator>();

        rangeUI.transform.localScale = new Vector3(status.radius, status.radius, 1f);
        bunkerWaveUI.localScale = new Vector3(status.radius, status.radius, 1f);
    }

    private void Update()
    {
        if (!gameManager.gameIsPaused)
        {
            if (Time.time - lastShootTime > status.fireRate && enemiesInRangeIndex > 0)
            {
                FireBunker();
                lastShootTime = Time.time;
            }
        }
    }

    void FireBunker()
    {
        anim.SetTrigger("PlayAnim");
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

                if (enemiesInRange[index].TakeDamage(status.damage) <= 0)
                {
                    CleanArray(index);
                    index--;
                }
            }
        }
    }

    private void CleanArray(int index)
    {
        while (index < enemiesInRangeIndex - 1)
        {
            enemiesInRange[index] = enemiesInRange[index + 1];
            index++;
        }
        if (enemiesInRangeIndex > 0)
        {
            enemiesInRangeIndex--;
            enemiesInRange[enemiesInRangeIndex] = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemiesInRange[enemiesInRangeIndex] = other.gameObject.GetComponent<Enemy>();
            enemiesInRangeIndex++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            for (int index = 0; index < enemiesInRangeIndex; index++)
                if (enemiesInRange[index] == other.gameObject.GetComponent<Enemy>())
                {
                    enemiesInRange[index].StopSlow();
                    CleanArray(index);
                }
        }
    }
}
