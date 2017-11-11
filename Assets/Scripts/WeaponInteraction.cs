using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteraction : MonoBehaviour {
    public bool isInteractable = false;
    public GameObject player;
    public GameObject iText;
    public GameObject swordPrefab;
    public bool isInteracting = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInteractable)
        {
            Debug.Log("2");
            GameObject childObject = Instantiate(swordPrefab,player.transform);
            childObject.GetComponent<Collider>().isTrigger = true;
            childObject.GetComponent<WeaponSword>().player = player;
            player.GetComponent<PlayerController>().weaponChild = childObject.transform;

            iText.SetActive(false);
            player.GetComponent<PlayerController>().interactedEntity = null;
            player.GetComponent<PlayerController>().iText.SetActive(false);
            player.GetComponent<PlayerController>().inRange = false;
            Destroy(transform.parent.gameObject);

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            iText.SetActive(true);
            Debug.Log("1");
            isInteractable = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = false;
            Debug.Log("3");
            player = other.gameObject;
            iText.SetActive(false);
            player.GetComponent<PlayerController>().interactedEntity = null;
            player.GetComponent<PlayerController>().iText.SetActive(false);
            player.GetComponent<PlayerController>().inRange = false;
            player = null;
        }
    }
}
