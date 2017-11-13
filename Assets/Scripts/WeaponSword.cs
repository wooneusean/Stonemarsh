using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSword : MonoBehaviour {
    //====================================//
    //            Swords etc.             //
    //====================================//
    public float knockback = 3f;
    public bool isAttacking = false;
    public int damage = 15;
    public Enemy enemyObject;
    public float distFromPlayer = 2f;
    public GameObject player;
    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
                other.GetComponent<Rigidbody>().AddForce(transform.forward * knockback, ForceMode.VelocityChange);
                other.gameObject.GetComponent<Enemy>().currentHealth -= damage;
                Debug.Log("Hit " + other.name);
        }
    }
}
