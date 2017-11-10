using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int currentHealth;
    public int maxHealth = 30;
    public float moveSpeed = 8f;
    public GameObject currentTarget;
    public Collider collCurrTarget;
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
	}
    private void FixedUpdate()
    {
        if (currentTarget != null)
        {
            collCurrTarget = currentTarget.GetComponent<Collider>();
            transform.LookAt(currentTarget.transform);
        }
    }
}
