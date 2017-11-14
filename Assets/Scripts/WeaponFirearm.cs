using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFirearm : MonoBehaviour {
    //====================================//
    //              Guns etc.             //
    //====================================//
    public float rateOfFire = 0.5f;
    public GameObject player;
    public PlayerController playerScript;
    public bool isAttacking = false;
    public float delay;
    public GameObject projectile;
    public float bulletSpeed = 100f;
    public int energyConsumption = 5;
    // Use this for initialization
    void Start () {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        playerScript = player.GetComponent<PlayerController>();
        rateOfFire = player.GetComponent<PlayerController>().attackDelay;
        delay = rateOfFire;

    }
    Vector3 prjPos;
    Quaternion prjRotation;
    Vector3 prjForce;

    // Update is called once per frame
    void Update () {
        if (playerScript == null)
        {
            playerScript = player.GetComponent<PlayerController>();
        }
        prjPos = transform.position + new Vector3(0f, -0.0065f, 0.004f);
        prjRotation = player.transform.rotation;
        delay -= 1 * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        prjForce = player.transform.forward * bulletSpeed;
        if (isAttacking && delay <= 0 && playerScript.localPlayerData.currentEnergy >= energyConsumption)
        {
            playerScript.localPlayerData.currentEnergy -= energyConsumption;
            playerScript.energyCooldown = 2f;
            delay = rateOfFire;
            Attack();
        }


    }
    void Attack()
    {
        var clone = Instantiate(projectile,prjPos,prjRotation);
        clone.GetComponent<Rigidbody>().AddForce(prjForce,ForceMode.VelocityChange);
        //Test for bool, if yes instantiate bullet with speed, control bool with animator.
    }
}
