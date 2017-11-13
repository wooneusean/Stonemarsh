using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordInteraction : MonoBehaviour {
    public bool isInteractable = false;
    public GameObject player;
    public PlayerController playerScript;
    public GameObject iText;
    public GameObject swordPrefab;
    public bool isInteracting = false;
    public bool playerHasWeapon = false;
    private void Update()
    {
        GameObject parentObject = transform.parent.gameObject;
        if (Input.GetKeyDown(KeyCode.E) && isInteractable && !playerHasWeapon)
        {
            playerScript = player.GetComponent<PlayerController>();
            GameObject childObject = Instantiate(swordPrefab,player.transform);
            childObject.GetComponent<WeaponSword>().player = player;
            childObject.GetComponent<Collider>().isTrigger = true;
            playerScript.localPlayerData.weaponChild = childObject.transform;
            playerScript.localPlayerData.hasWeapon = true;
            playerScript.localPlayerData.droppedWeaponObject = transform.parent.gameObject;
            playerScript.localPlayerData.droppedWeaponObjectPath = playerScript.GetPrefabPath(transform.parent.gameObject);
            parentObject.transform.parent = player.transform;
            parentObject.transform.localPosition = Vector3.zero;
            parentObject.SetActive(false);

            iText.SetActive(false);
            playerScript.interactedEntity = null;
            playerScript.iText.SetActive(false);
            playerScript.inRange = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            iText = player.GetComponent<PlayerController>().iText;
            playerHasWeapon = player.GetComponent<PlayerController>().localPlayerData.hasWeapon;
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
