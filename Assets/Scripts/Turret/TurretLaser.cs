using UnityEngine;

public class TurretLaser : MonoBehaviour
{
    public TurretStatus status;
    public string turretName;       //Used to find what build menu to activate

    //Laser variables
    private LineRenderer lineRenderer;

    //Default variables
    public Transform firePoint;
    public Transform partToRotate;
    public SpriteRenderer rangeUI;

    private Enemy enemyScript;
    private Quaternion defaultRotation;
    private LevelManager gameManager;
    private SceneDataRetainer dataRetainer;

    private float lastChangeTargetTime = 0f;

    private float range;
    private float damage;

    private void Start()
    {
        dataRetainer = SceneDataRetainer.instance;
        range = status.radius * dataRetainer.laserMultipliers[0];
        damage = status.damage * dataRetainer.laserMultipliers[1];

        gameManager = LevelManager.instance;
        lineRenderer = GetComponent<LineRenderer>();
        defaultRotation = new Quaternion(0f, 180f, 0f, 0f).normalized;

        rangeUI.transform.localScale = new Vector3(range, range, 1f);
    }

    private void Update()
    {
        if (!gameManager.gameIsPaused)
        {
            //Rotate turret towards the target
            LockOnTarget();

            //If we can shoot and we have a target, then we shoot
            FireLaser();
        }
    }

    void FireLaser()
    {
        if (enemyScript != null)
        {
            lineRenderer.SetPosition(0, firePoint.position - transform.position);
            lineRenderer.SetPosition(1, enemyScript.transform.position - transform.position);

            if (status.canSlow)
            {
                enemyScript.Slow();
            }

            enemyScript.TakeDamage(damage * Time.deltaTime);
        }
    }

    //Rotate turret so it looks at target
    void LockOnTarget()
    {
        if (lastChangeTargetTime + 0.5f < Time.time)
        {
            Enemy[] enemyInstances = GameObject.FindObjectsOfType<Enemy>();

            float distance, aux = range;
            if(status.canSlow && enemyScript != null)
                enemyScript.StopSlow();
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
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
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
