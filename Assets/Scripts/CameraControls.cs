using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {
    public GameObject target;
    public float damping = 1;
    public Vector3 offset;
    public float maxY = 2f;
    public float minY = -2f;
    public float maxZ = 10f;
    public float minZ = 2f;
    public float camYSens = 10f;

    void LateUpdate()
    {
        float change = Input.GetAxisRaw("Mouse Y") * camYSens * Time.deltaTime;
        float y = change;
        float z = -(change * 2f);
        offset = offset + (new Vector3(0,y,z));

        if (offset.y >= maxY)
        {
            offset.y = maxY;
        }
        else if (offset.y <= minY)
        {
            offset.y = minY;
        }
        if (offset.z >= maxZ)
        {
            offset.z = maxZ;
        }
        else if (offset.z <= minZ)
        {
            offset.z = minZ;
        }

        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = target.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = target.transform.position - (rotation * offset);
        transform.LookAt(target.transform);
    }
}
