using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponInteraction : MonoBehaviour {
    public bool isInteractable = false;
    public GameObject player;
    public PlayerController playerScript;
    public GameObject iText;
    public GameObject weapPrefab;
    public bool isInteracting = false;
    public bool playerHasWeapon = false;
    public float playerDistance = 1.2f;
    public float dropTime = 1.5f;
    private void Update()
    {
        dropTime -= Time.deltaTime;
        if (player != null)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            playerScript = player.GetComponent<PlayerController>();
            if (isInteractable && !playerHasWeapon && (distanceFromPlayer <= playerDistance) && dropTime <= 0)
            {
                equipWeapon();
            }
        }
    }
    void equipWeapon()
    {

        GameObject parentObject = transform.parent.gameObject;
        GameObject childObject = Instantiate(weapPrefab, player.transform);
        childObject.name = childObject.name.Replace("(Clone)", "");
        playerScript.localPlayerData.weaponChild = childObject.transform;
        playerScript.localPlayerData.weaponChildPath = GetPrefabPath(weapPrefab);
        playerScript.localPlayerData.hasWeapon = true;
        playerScript.localPlayerData.droppedWeaponObject = transform.parent.gameObject;
        playerScript.localPlayerData.droppedWeaponObjectPath = GetPrefabPathWithParent(transform.parent.gameObject);
        parentObject.transform.parent = player.transform;
        parentObject.transform.localPosition = Vector3.zero;
        parentObject.SetActive(false);

        iText.SetActive(false);
        playerScript.interactedEntity = null;
        playerScript.iText.SetActive(false);
        playerScript.inRange = false;
    }
    public string GetPrefabPath(GameObject prefab)
    {
        Object go = PrefabUtility.GetPrefabObject(prefab);
        string prefabPath = AssetDatabase.GetAssetPath(go);
        return prefabPath.Replace("Assets/Resources/", "").Replace(".prefab", ""); ;
    }
    public string GetPrefabPathWithParent(GameObject prefab)
    {
        Object go = PrefabUtility.GetPrefabParent(prefab);
        string prefabPath = AssetDatabase.GetAssetPath(go);
        return prefabPath.Replace("Assets/Resources/", "").Replace(".prefab", ""); ;
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
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerHasWeapon = player.GetComponent<PlayerController>().localPlayerData.hasWeapon;
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
