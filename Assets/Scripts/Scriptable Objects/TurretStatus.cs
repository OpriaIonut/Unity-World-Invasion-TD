using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject_Turret", menuName = "ScriptableObjects/TurretStatus", order = 1)]
public class TurretStatus : ScriptableObject {
    
    public GameObject bulletPrefab;
    public bool canSlow = false;
    public bool canStun = false;
    public float probability = 20f;
    public float damage = 10f;
    public float fireRate = 0.75f;
    public float radius = 10f;
    public float rotationSpeed = 10f;
    public int price;
}
