using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroScript : MonoBehaviour {
    public GameObject parentObject;
    public Enemy parentScript;
    private void Start()
    {
        parentScript = parentObject.GetComponent<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parentScript.currentTarget = other.gameObject;
        }
    }
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
    private void OnTriggerExit(Collider other)
    {
        parentScript.currentTarget = null;
    }
}
