using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalControl : MonoBehaviour {
    public static GlobalControl Instance;
    public string savedJson;
    public PlayerStatistics savedPlayerData = new PlayerStatistics();
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        Debug.Log(savedJson);
    }
}
