using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {

    PlayerController player;
    [Range(1, 5)]
    public float magnetSpeed;
    public float rotateSpeed;
    bool inRange = false;
    public bool isCurrency = false;
    int value;
    public bool isHealth = false;
    int health;
    public bool isEnergy = false;
    int energy;

	// Use this for initialization
	void Start () {
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
            health = Mathf.RoundToInt((float)0.03 * player.maxHealth);
            Debug.Log(health);
        }
        if (isEnergy == true)
        {
            energy = 5;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!(Vector3.Distance(transform.position,player.transform.position) <= .5f))
        {
            if (inRange)
            {
                PickUp();
            }
        }
        else
        {
            player.localPlayerData.money += value;
            player.localPlayerData.currentHealth += health;
            player.localPlayerData.currentEnergy += energy;
            Destroy(gameObject);
        }
	}
    void PickUp()
    {
        transform.RotateAround(player.transform.position,Vector3.up, rotateSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, magnetSpeed * (Vector3.Distance(transform.position, player.transform.position)) * Time.deltaTime);
        //transform.position = Vector3.Lerp(transform.position, player.transform.position, (multiplier * (Vector3.Distance(transform.position,player.transform.position))) * Time.deltaTime);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

}
