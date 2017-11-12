using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

public class WarpScript : MonoBehaviour {
    [Header("Scene Settings")]
    public string sceneToLoad;
    public bool isInWarp = false;
    [Header("Player Settings")]
    public GameObject player;
    public PlayerController playerScript;
    [Header("UI Settings")]
    public GameObject LoadingScreen;
    public GameObject interactionText;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        interactionText = playerScript.iText;
        LoadingScreen = playerScript.LoadingScreen;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInWarp)
        {
            StartCoroutine(LoadSceneAsync(sceneToLoad));
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            playerScript.SavePlayer();
            isInWarp = true;
            interactionText.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionText.SetActive(false);
            isInWarp = false;
        }
    }
    IEnumerator LoadSceneAsync(string scene)
    {
        // The Application loads the Scene in the background at the same time as the current Scene.
        //This is particularly good for creating loading screens. You could also load the scene by build //number.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            
            LoadingScreen.SetActive(true);
            yield return null;
        }
        LoadingScreen.SetActive(false);
    }
}
