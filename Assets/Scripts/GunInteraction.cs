using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInteraction : MonoBehaviour {
    public bool isInteractable = false;
    public GameObject player;
    public GameObject iText;
    public GameObject gunPrefab;
    public bool isInteracting = false;
    public bool playerHasWeapon = false;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        iText = player.GetComponent<PlayerController>().iText;
    }
    private void Update()
    {
        GameObject parentObject = transform.parent.gameObject;
        if (Input.GetKeyDown(KeyCode.E) && isInteractable && !playerHasWeapon)
        {
            GameObject childObject = Instantiate(gunPrefab,player.transform);
            childObject.GetComponent<WeaponFirearm>().player = player;
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
            iText.SetActive(false);
            player.GetComponent<PlayerController>().interactedEntity = null;
            player.GetComponent<PlayerController>().iText.SetActive(false);
            player.GetComponent<PlayerController>().inRange = false;
            player = null;
        }
    }
}
