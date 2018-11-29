using UnityEngine;

public class Turret : MonoBehaviour
{
    public TurretStatus status;
    public string turretName;       //Used to find what build menu to activate

    //Laser variables
    private LineRenderer lineRenderer;

    //Bunker variables
    private Animator anim;
    public Transform bunkerWaveUI;

    //Default variables
    public Transform firePoint;
    public Transform partToRotate;
    public SpriteRenderer rangeUI;

    private Enemy[] enemiesInRange = new Enemy[25];
    private short enemiesInRangeIndex = 0;
    private Transform target;
    private Quaternion defaultRotation;
    private Enemy enemyScript;
    private GameManager gameManager;

    private float lastShootTime = 0f;
    private float lastChangeTargetTime = 0f;

    private void Start()
    {
        gameManager = GameManager.instance;
        anim = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
        defaultRotation = new Quaternion(0f, 180f, 0f, 0f).normalized;
        InvokeRepeating("CheckForEmptyTargets", 10f, 2f);

        rangeUI.transform.localScale = new Vector3(status.radius, status.radius, 1f);
        if(bunkerWaveUI != null)
            bunkerWaveUI.localScale = new Vector3(status.radius, status.radius, 1f);
    }

    private void Update()
    {
        if (!gameManager.gameIsPaused)
        {
            if (status.isBunker)
            {
                if (Time.time - lastShootTime > status.fireRate && enemiesInRangeIndex > 0)
                {
                    FireBunker();
                    lastShootTime = Time.time;
                }
                return;
            }

            //Rotate turret towards the target
            LockOnTarget();

            //If we can shoot and we have a target, then we shoot
            if (status.isLaser)
            {
                FireLaser();
            }
            else if (Time.time - lastShootTime > status.fireRate)
            {
                FireBullet();
            }
        }
    }

    void FireBunker()
    {
        anim.SetTrigger("PlayAnim");

        CheckForEmptyTargets();

        for (int index = 0; index < enemiesInRangeIndex; index++)
        {
            //Sometimes, if we kill an enemy, the script is still selected as null, so we need to check beforehand if we killed him or not
            if (enemiesInRange[index] != null)
            {
                if (status.canSlow)
                {
                    enemiesInRange[index].Slow();
                }
                if(status.canStun)
                {
                    float randomValue = Random.RandomRange(0f, 100f);
                    if(randomValue <= status.probability)
                    {
                        enemiesInRange[index].Stun();
                    }
                }

                enemiesInRange[index].TakeDamage(status.damage);
            }
        }
    }

    void FireLaser()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > status.radius)
            {
                SelectNextTarget();
            }
        }

        if (target != null)
        {
            lineRenderer.SetPosition(0, firePoint.position - transform.position);
            lineRenderer.SetPosition(1, target.position - transform.position);

            if (status.canSlow || status.canStun)
            {
                enemyScript.Slow();
            }

            enemyScript.TakeDamage(status.damage * Time.deltaTime);
        }
    }

    //Instantiate bullet and give it the required status
    void FireBullet()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > status.radius)
            {
                SelectNextTarget();
            }
        }

        if (target != null)
        {
            GameObject clone = Instantiate(status.bulletPrefab);
            clone.transform.position = firePoint.position;
            BulletMovement cloneScript = clone.GetComponent<BulletMovement>();
            cloneScript.SetTarget(target);
            cloneScript.SetDamage(status.damage);

            lastShootTime = Time.time;
        }
    }

    //Rotate turret so it looks at target
    void LockOnTarget()
    {
        if (target == null)
        {
            SelectNextTarget();
            if (status.isLaser)
            {
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
            }
            partToRotate.rotation = Quaternion.Slerp(partToRotate.rotation, defaultRotation, status.rotationSpeed * Time.deltaTime);
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - partToRotate.position);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, targetRotation, status.rotationSpeed * Time.deltaTime).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void DeselectEnemyForBunker(int index)
    {
        enemiesInRange[index].StopSlow();
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

    private void CheckForEmptyTargets()
    {
        if (!gameManager.gameIsPaused)
        {
            bool startOverwriting = false;
            for (int index = 0; index < enemiesInRange.Length - 1; index++)
            {
                if (enemiesInRange[index] == null)
                {
                    if (index < enemiesInRangeIndex)
                    {
                        startOverwriting = true;
                    }
                    enemiesInRange[index] = null;
                }
                if (startOverwriting)
                {
                    enemiesInRange[index] = enemiesInRange[index + 1];

                }
            }
            if (startOverwriting)
                enemiesInRangeIndex--;
        }
    }

    private void SelectNextTarget()
    {
        if (lastChangeTargetTime + 1f < Time.time)
        {
            if (status.canSlow && enemyScript != null)
            {
                enemyScript.StopSlow();
            }
            target = null;
            enemyScript = null;

            float currentDistance = Mathf.Infinity;
            for (int index = 0; index < enemiesInRangeIndex; index++)
            {
                if (enemiesInRange[index] != null)
                {
                    float aux = Vector3.Distance(transform.position, enemiesInRange[index].transform.position);
                    if (aux < currentDistance)
                    {
                        target = enemiesInRange[index].transform;
                        enemyScript = enemiesInRange[index];
                        currentDistance = aux;
                    }
                }
            }
            lastChangeTargetTime = Time.time;
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
        if(other.gameObject.tag == "Enemy")
        {
            for (int index = 0; index < enemiesInRangeIndex; index++)
                if (enemiesInRange[index] == other.gameObject.GetComponent<Enemy>())
                {
                    DeselectEnemyForBunker(index);
                }
        }
    }

    //Draw range in scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, status.radius);
    }
}
