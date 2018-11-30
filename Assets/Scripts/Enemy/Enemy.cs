﻿using UnityEngine;

public class Enemy : MonoBehaviour {

    public EnemyStatus enemyStatus;
    public float health;
    public float speed;

    private BuildManager buildManager;
    private LevelManager gameManager;
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
        buildManager.AddMoney(enemyStatus.moneyValue);
        Destroy(gameObject);
    }

    public void Slow()
    {
        speed = enemyStatus.speed / 2f;
    }

    public void StopSlow()
    {
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
        gameManager = LevelManager.instance;
        gameManager.LoseLives(enemyStatus.damageValue);
        Destroy(gameObject);
    }
}
