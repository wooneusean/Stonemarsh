using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int currentHealth;
    public int maxHealth = 30;
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
}
