using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInteraction : MonoBehaviour {
    public bool isInteractable = false;
    public GameObject player;
    public GameObject iText;
    public GameObject gunPrefab;
    public bool isInteracting = false;

    private void Update()
    {
        bool playerHasWeapon = player.GetComponent<PlayerController>().hasWeapon;
        if (Input.GetKeyDown(KeyCode.E) && isInteractable && !playerHasWeapon)
        {
            Debug.Log("2");
            GameObject childObject = Instantiate(gunPrefab,player.transform);
            childObject.GetComponent<WeaponFirearm>().player = player;
            player.GetComponent<PlayerController>().weaponChild = childObject.transform;
            player.GetComponent<PlayerController>().hasWeapon = true;

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
