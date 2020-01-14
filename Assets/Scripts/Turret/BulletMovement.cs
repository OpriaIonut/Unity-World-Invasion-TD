using UnityEngine;

public class BulletMovement : MonoBehaviour {

    public float speed = 20f;
    
    [Header("Rocket variables")]
    public GameObject rangeUI;
    public GameObject gfx;
    private bool stopMoving = false;

    [HideInInspector]
    public float damageValue;
    private Transform target;
    private LevelManager gameManager;

    private void Start()
    {
        gameManager = LevelManager.GetInstance();
    }

    private void Update()
    {
        if (!gameManager.gameIsPaused && !stopMoving)
        {
            if (target == null)
            {
                Destroy(gameObject);
            }
            // Look and move towards target
            transform.LookAt(target);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (gameObject.tag != "Rocket")
            {
                Enemy collisionScript = other.gameObject.GetComponent<Enemy>();
                collisionScript.TakeDamage(damageValue);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject, 0.8f);
                gfx.SetActive(false);
                rangeUI.SetActive(true);
                stopMoving = true;
            }
        }
    }

    //Set the target externally
    public void SetTarget(Transform element)
    {
        target = element;
    }

    //Set the damage externally
    public void SetDamage(float damage)
    {
        damageValue = damage;
    }
}
