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
    public GameObject player;
    PlayerController playerScript;
    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<PlayerController>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
                other.GetComponent<Rigidbody>().AddForce(transform.forward * knockback, ForceMode.VelocityChange);
                other.gameObject.GetComponent<Enemy>().currentHealth -= Mathf.RoundToInt(damage * Crit(playerScript.localPlayerData.critChance,playerScript.localPlayerData.critMultiplier));
                Debug.Log(other.gameObject.GetComponent<Enemy>().currentHealth + " HP");
        }
    }
    float Crit(float percent, float critMultiplier)
    {
        if (Random.value <= (percent / 100f))
        {
            return critMultiplier;
        }
        return 1f;
    }
}
