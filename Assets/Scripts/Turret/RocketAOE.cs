using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAOE : MonoBehaviour {

    public BulletMovement bulletScript;
    private bool stopAOE = false;

    private void Start()
    {
        Invoke("StopAOE", 0.2f);
    }

    private void StopAOE()
    {
        stopAOE = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && !stopAOE)
        {
            other.GetComponent<Enemy>().TakeDamage(bulletScript.damageValue);
        }
    }
}
