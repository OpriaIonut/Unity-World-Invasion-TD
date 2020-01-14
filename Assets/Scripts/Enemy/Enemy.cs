using UnityEngine;

public class Enemy : MonoBehaviour {

    public EnemyStatus enemyStatus;
    public float health;
    public float speed;
    public bool isSlowed = false;
    
    private LevelManager gameManager;
    private bool isStunned = false;
    private float stunTime = 0f;

    private void Start()
    {
        gameManager = LevelManager.GetInstance();
        speed = enemyStatus.speed;
        health = enemyStatus.health * (1f + gameManager.currentLevel / 15f);
    }

    private void Update()
    {
        if(isStunned && stunTime + 1f < Time.time)
        {
            isStunned = false;
            speed = enemyStatus.speed;
        }
    }

    public void TakeDamage(float ammount)
    {
        health -= ammount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        BuildManager.GetInstance().AddMoney(enemyStatus.moneyValue);
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
        gameManager.LoseLives(enemyStatus.damageValue);
        Destroy(gameObject);
    }
}
