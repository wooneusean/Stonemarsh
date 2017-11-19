using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int currentHealth;
    public int damage = 2;
    public int maxHealth = 30;
    public float moveSpeed = 8f;
    public int difficulty = 1;
    public int expDrop = 25;
    public GameObject currentTarget;
    public Collider collCurrTarget;
    GameObject coinOrb;
    GameObject healthOrb;
    GameObject energyOrb;
    public float distFromPlayer = 1.2f;
    public float maxAttackTime = 1.2f;
    public float attackTime;
    public bool inRange = false;
    void Start()
    {
        coinOrb = Resources.Load<GameObject>("Prefabs/Prefab_Objects/CoinOrb");
        healthOrb = Resources.Load<GameObject>("Prefabs/Prefab_Objects/HealthOrb");
        energyOrb = Resources.Load<GameObject>("Prefabs/Prefab_Objects/EnergyOrb");
        currentHealth = maxHealth;
        attackTime = maxAttackTime;

    }
    // Update is called once per frame
    void Update () {
		if (currentHealth <= 0)
        {
            Die();
        }
        if (currentTarget != null)
        {
            attackTime -= Time.deltaTime;
            if ((attackTime <= 0) && inRange)
            {
                currentTarget.GetComponent<PlayerController>().localPlayerData.currentHealth -= damage;
                attackTime = maxAttackTime;
            }
            if (Vector3.Distance(transform.position, currentTarget.transform.position) >= distFromPlayer)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                inRange = false;
                if (Vector3.Distance(transform.position, currentTarget.transform.position) <= distFromPlayer)
                {
                    inRange = true;
                }
            }
            collCurrTarget = currentTarget.GetComponent<Collider>();
            Quaternion rotation = Quaternion.LookRotation(currentTarget.transform.position - transform.position);
            rotation.x = 0f;
            rotation.z = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
        }
    }
    int GetDropAmmount()//range from 0.0 to 1.0
    {
        float p = Random.Range(1f,100f);
        if (p <= 12.5f)
        {
            return 8;
        }
        if (p <= 25f)
        {
            return 4;
        }
        if (p <= 50f)
        {
            return 2;
        }
        if (p <= 100f)
        {
            return 1;
        }
        return 0;
    }
    void Die()
    {
        //Inst. go
        for (int i = 0; i < GetDropAmmount() * difficulty; i++)
        {
            Transform tr = Instantiate(coinOrb, transform.position, Quaternion.identity).GetComponent<Transform>();
            tr.position = tr.position + new Vector3(Random.value, Random.value, Random.value);
        }
        for (int i = 0; i < GetDropAmmount() * difficulty; i++)
        {
            Transform tr = Instantiate(healthOrb, transform.position, Quaternion.identity).GetComponent<Transform>();
            tr.position = tr.position + new Vector3(Random.value, Random.value, Random.value);
        }
        for (int i = 0; i < GetDropAmmount() * difficulty; i++)
        {
            Transform tr = Instantiate(energyOrb, transform.position, Quaternion.identity).GetComponent<Transform>();
            tr.position = tr.position + new Vector3(Random.value, Random.value, Random.value);
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddExp(expDrop);
        Destroy(gameObject);
    }
}
