using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroScript : MonoBehaviour {
    public GameObject parentObject;
    public Enemy parentScript;
    private void Start()
    {
        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parentScript.currentTarget = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            parentScript.currentTarget = null;
        }
    }
}
