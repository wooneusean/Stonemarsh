using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordInteraction : MonoBehaviour {
    public bool isInteractable = false;
    public GameObject player;
    public GameObject iText;
    public GameObject swordPrefab;
    public bool isInteracting = false;
    public bool playerHasWeapon = false;

    private void Update()
    {
        GameObject parentObject = transform.parent.gameObject;
        if (Input.GetKeyDown(KeyCode.E) && isInteractable && !playerHasWeapon)
        {
            
            GameObject childObject = Instantiate(swordPrefab,player.transform);
            childObject.GetComponent<Collider>().isTrigger = true;
            childObject.GetComponent<WeaponSword>().player = player;
            player.GetComponent<PlayerController>().weaponChild = childObject.transform;
            player.GetComponent<PlayerController>().hasWeapon = true;
            player.GetComponent<PlayerController>().droppedWeaponObject = transform.parent.gameObject;
            parentObject.transform.parent = player.transform;
            parentObject.transform.localPosition = Vector3.zero;
            parentObject.SetActive(false);

            iText.SetActive(false);
            player.GetComponent<PlayerController>().interactedEntity = null;
            player.GetComponent<PlayerController>().iText.SetActive(false);
            player.GetComponent<PlayerController>().inRange = false;

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            iText = player.GetComponent<PlayerController>().iText;
            player = other.gameObject;
            playerHasWeapon = player.GetComponent<PlayerController>().hasWeapon;
            iText.SetActive(true);
            isInteractable = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = false;
            player = other.gameObject;
            iText = player.GetComponent<PlayerController>().iText;
            player.GetComponent<PlayerController>().interactedEntity = null;
            player.GetComponent<PlayerController>().iText.SetActive(false);
            player.GetComponent<PlayerController>().inRange = false;
            player = null;
        }
    }
}
