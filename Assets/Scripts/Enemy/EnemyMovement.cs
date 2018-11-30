using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour {

    private float arrivalRadius = 0.5f;             // Used to smooth arrival to waypoint
    private Transform target;                       // Used to move between waypoints
    private WaypointsTransform waypointsScript;     // Get the waypoints
    private Enemy enemyScript;                      // Get status and Reached destination
    private LevelManager gameManager;                // Needed to know if game is paused or not
    private int currentIndex = 0;                   // Index used for traveling between waypoints

    private void Start()
    {
        gameManager = LevelManager.instance;
        waypointsScript = FindObjectOfType<WaypointsTransform>().GetComponent<WaypointsTransform>();
        enemyScript = GetComponent<Enemy>();
        target = waypointsScript.waypointsTransform[0];
        transform.LookAt(target);
    }

    private void FixedUpdate()
    {
        if(!gameManager.gameIsPaused)
            transform.Translate(Vector3.forward * enemyScript.speed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (!gameManager.gameIsPaused)
        {
            if (Vector3.Distance(transform.position, target.position) < arrivalRadius)
            {
                currentIndex++;

                if (currentIndex == waypointsScript.waypointsTransform.Length)
                {
                    enemyScript.ReachedDestination();
                }
                else
                {
                    target = waypointsScript.waypointsTransform[currentIndex];
                    transform.LookAt(target);
                }
            }
        }
    }
}
