using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunInteraction : MonoBehaviour {
    public bool isInteractable = false;
    public GameObject player;
    public PlayerController playerScript;
    public GameObject iText;
    public GameObject gunPrefab;
    public bool isInteracting = false;
    public bool playerHasWeapon = false;
    private void Update()
    {
        GameObject parentObject = transform.parent.gameObject;
        if (Input.GetKeyDown(KeyCode.E) && isInteractable && !playerHasWeapon)
        {
            playerScript = player.GetComponent<PlayerController>();
            GameObject childObject = Instantiate(gunPrefab,player.transform);
            childObject.name = childObject.name.Replace("(Clone)", "");
            childObject.GetComponent<WeaponFirearm>().player = player;
            playerScript.localPlayerData.weaponChild = childObject.transform;
            playerScript.localPlayerData.weaponChildPath = GetPrefabPath(gunPrefab);
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
