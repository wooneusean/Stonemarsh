using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour {
    public GameObject go;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(go, new Vector3(transform.position.x + Random.Range(1,3), 1, transform.position.z + Random.Range(1, 3)),transform.rotation);
        }
    }
}
