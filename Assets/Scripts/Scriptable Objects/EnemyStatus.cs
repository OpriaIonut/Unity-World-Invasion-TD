using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject_Enemy", menuName = "ScriptableObjects/EnemyStatus", order = 1)]
public class EnemyStatus : ScriptableObject
{
    public float health;
    public int speed;
    public int damageValue;
    public int moneyValue;
}
