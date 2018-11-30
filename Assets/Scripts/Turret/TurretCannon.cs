using UnityEngine;

public class TurretCannon : MonoBehaviour
{
    public TurretStatus status;
    public string turretName;       //Used to find what build menu to activate

    //Default variables
    public Transform firePoint;
    public Transform partToRotate;
    public SpriteRenderer rangeUI;

    private Enemy enemyScript;
    private Quaternion defaultRotation;
    private LevelManager gameManager;

    private float lastShootTime = 0f;
    private float lastChangeTargetTime = 0f;

    private void Start()
    {
        gameManager = LevelManager.instance;
        defaultRotation = new Quaternion(0f, 180f, 0f, 0f).normalized;

        rangeUI.transform.localScale = new Vector3(status.radius, status.radius, 1f);
    }

    private void Update()
    {
        if (!gameManager.gameIsPaused)
        {
            //Rotate turret towards the target
            LockOnTarget();

            //If we can shoot and we have a target, then we shoot
            if (Time.time - lastShootTime > status.fireRate)
            {
                FireBullet();
            }
        }
    }

    //Instantiate bullet and give it the required status
    void FireBullet()
    {
        if (enemyScript != null)
        {
            GameObject clone = Instantiate(status.bulletPrefab);
            clone.transform.position = firePoint.position;
            BulletMovement cloneScript = clone.GetComponent<BulletMovement>();
            cloneScript.SetTarget(enemyScript.transform);
            cloneScript.SetDamage(status.damage);

            lastShootTime = Time.time;
        }
    }

    //Rotate turret so it looks at target
    void LockOnTarget()
    {
        if (lastChangeTargetTime + 1f < Time.time)
        {
            Enemy[] enemyInstances = GameObject.FindObjectsOfType<Enemy>();

            float distance, aux = status.radius;
            enemyScript = null;
            foreach (Enemy enemy in enemyInstances)
            {
                distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance <= aux)
                {
                    aux = distance;
                    enemyScript = enemy;
                    lastChangeTargetTime = Time.time;
                }
            }
        }

        if (enemyScript == null)
        {
            partToRotate.rotation = Quaternion.Slerp(partToRotate.rotation, defaultRotation, status.rotationSpeed * Time.deltaTime);
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(enemyScript.transform.position - partToRotate.position);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, targetRotation, status.rotationSpeed * Time.deltaTime).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }
}
