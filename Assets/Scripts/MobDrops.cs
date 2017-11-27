using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MobDrops : MonoBehaviour
{

    PlayerController player;
    public float magnetSpeed = 10f;
    public float rotateSpeed = 600f;
    bool inRange = false;
    public bool isCurrency = false;
    int value;
    public bool isHealth = false;
    int health;
    public bool isEnergy = false;
    int energy;

    // Use this for initialization
    void Start()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        if (isCurrency == true)
        {
            value = Random.Range(1, 30);
        }
        if (isHealth == true)
        {
            health = Mathf.RoundToInt(0.03f * player.maxHealth);
        }
        if (isEnergy == true)
        {
            energy = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!(Vector3.Distance(transform.position, player.transform.position) <= .5f))
        {
            if (inRange)
            {
                Attract();
            }
        }
    }
    void PickUp()
    {
        player.localPlayerData.money += value;
        player.localPlayerData.currentHealth += health;
        player.localPlayerData.currentEnergy += energy;
        Destroy(gameObject);
    }
    void Attract()
    {
        transform.RotateAround(player.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, magnetSpeed * (Vector3.Distance(transform.position, player.transform.position)) * Time.deltaTime);
        //transform.position = Vector3.Lerp(transform.position, player.transform.position, (multiplier * (Vector3.Distance(transform.position,player.transform.position))) * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PickUp();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }

    }
}
