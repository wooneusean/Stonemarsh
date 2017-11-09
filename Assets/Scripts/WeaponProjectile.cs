using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour {
    //====================================//
    //           Bullets etc.             //
    //====================================//
    public float knockback = 0.5f;
    public int damage = 15;
    public Enemy enemyObject;
    public float destroyTimer = 5f;
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Enemy"))
        {
            Destroy(gameObject, destroyTimer);
        }
        else
        {
            collision.collider.GetComponent<Rigidbody>().AddForce(-collision.collider.transform.forward * knockback, ForceMode.VelocityChange);
            enemyObject = collision.collider.gameObject.GetComponent<Enemy>();
            enemyObject.currentHealth -= damage;
            Debug.Log("Hit " + collision.collider.name);
            Destroy(gameObject);
        }
    }
}
