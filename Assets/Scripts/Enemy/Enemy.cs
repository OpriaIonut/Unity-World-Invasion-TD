using UnityEngine;

public class Enemy : MonoBehaviour {

    public EnemyStatus enemyStatus;
    public float health;
    public float speed;

    private BuildManager buildManager;
    private GameManager gameManager;
    private bool isSlowed = false;
    private bool isStunned = false;
    private float stunTime = 0f;

    private void Start()
    {
        speed = enemyStatus.speed;
        health = enemyStatus.health;
        buildManager = BuildManager.instance;
    }

    private void Update()
    {
        if(isStunned && stunTime + 1f < Time.time)
        {
            isStunned = false;
            speed = enemyStatus.speed;
        }
    }

    public float TakeDamage(float ammount)
    {
        health -= ammount;
        if (health <= 0)
        {
            Die();
        }
        return health;
    }

    void Die()
    {
        buildManager.AddMoney(enemyStatus.moneyValue);
        Destroy(gameObject);
    }

    public void Slow()
    {
        isSlowed = true;
        speed = enemyStatus.speed / 2f;
    }

    public void StopSlow()
    {
        isSlowed = false;
        speed = enemyStatus.speed;
    }

    public void Stun()
    {
        isStunned = true;
        speed = 0;
        stunTime = Time.time;
    }

    public void ReachedDestination()
    {
        gameManager = GameManager.instance;
        gameManager.LoseLives(enemyStatus.damageValue);
        Destroy(gameObject);
    }
}
