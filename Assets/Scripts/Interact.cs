using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {
    public GameObject Interactable;
    public GameObject player;
    public bool isInteracting = false;
	// Use this for initialization
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            player.GetComponent<PlayerController>().interactedEntity = transform.parent;
            player = other.gameObject;
            player.GetComponent<PlayerController>().iText.SetActive(true);
            player.GetComponent<PlayerController>().inRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().interactedEntity = null;
            player = other.gameObject;
            player.GetComponent<PlayerController>().iText.SetActive(false);
            player.GetComponent<PlayerController>().inRange = false;
            player = null;
        }
    }
}
