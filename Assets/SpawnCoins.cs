using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoins : MonoBehaviour {
    public GameObject coins;

	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.F))
        {
            Instantiate(coins,new Vector3(transform.position.x + Random.Range(1,3), 1, transform.position.z + Random.Range(1, 3)),transform.rotation);
        }
    }
}
