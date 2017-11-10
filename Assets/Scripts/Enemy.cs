using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int currentHealth;
    public int maxHealth = 30;
    public float moveSpeed = 8f;
    public GameObject currentTarget;
    public Collider collCurrTarget;
    public float distFromPlayer = 1.2f;
    void Start()
    {

        currentHealth = maxHealth;
    }
    // Update is called once per frame
    void Update () {
		if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        if (Vector3.Distance(transform.position, currentTarget.transform.position) >= distFromPlayer)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, currentTarget.transform.position) <= distFromPlayer)
            {

            }

        }
        if (currentTarget != null)
        {
            collCurrTarget = currentTarget.GetComponent<Collider>();
            Quaternion rotation = Quaternion.LookRotation(currentTarget.transform.position - transform.position);
            rotation.x = 0f;
            rotation.z = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
        }
    }
}
