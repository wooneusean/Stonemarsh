using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int currentHealth;
    public int maxHealth = 30;
    public float moveSpeed = 8f;
    public GameObject currentTarget;
    public Collider collCurrTarget;
    public float offsetPosFromTarget = 1.2f;
    public bool isMoving = false;
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
        if (currentTarget != null) { 
            if (isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position * offsetPosFromTarget, moveSpeed * Time.deltaTime);
            }
            if (transform.position == (currentTarget.transform.position * offsetPosFromTarget))
            {
                isMoving = false;
            }
        }
    }

    private void FixedUpdate()
    {
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
