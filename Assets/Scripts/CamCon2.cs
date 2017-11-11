using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCon2 : MonoBehaviour {
    [Header("[TARGET]")]
    [SerializeField]
    private Transform target = null; // manually assign target transform 
    [SerializeField] private string targetTag; //Or just insert your target tag
    [SerializeField] private string[] ignoreTag; //List of ignored tags so the raycast can pass through on them
    [Range(0, 5.0f)]
    [SerializeField]
    private float invokeUpdateRate = 0.3f;
    [Header("[ADJUSTMENT]")]
    [SerializeField]
    private float distance = 4.0f;
    [SerializeField] private float height = 0.35f;
    [SerializeField] private float damping = 2.0f;
    [SerializeField] private bool smoothRotation = true;
    [SerializeField] private float rotationDamping = 3.0f;

    [SerializeField] private Vector3 targetLookAtOffset; // allows offsetting of camera lookAt, very useful for low bumper heights

    [SerializeField] private float bumperDistanceCheck = 2.5f; // length of bumper ray
    [SerializeField] private float bumperCameraHeight = 1.0f; // adjust camera height while bumping
    [SerializeField] private Vector3 bumperRayOffset; // allows offset of the bumper ray from target origin

    private void Start()
    {
        if (target == null) InvokeRepeating("FindTarget", 0.5f, invokeUpdateRate);
    }

    private void FindTarget()
    {
        if (target == null)
        {
            GameObject camTarget = GameObject.FindGameObjectWithTag(targetTag);
            if (camTarget != null)
            {
                target = camTarget.transform;
            }
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 wantedPosition = target.TransformPoint(0, height, -distance);

            // check to see if there is anything behind the target
            RaycastHit hit;
            Vector3 back = target.transform.TransformDirection(-1 * Vector3.forward);

            // cast the bumper ray out from rear and check to see if there is anything behind
            if (Physics.Raycast(target.TransformPoint(bumperRayOffset), back, out hit, bumperDistanceCheck)
                && hit.transform != target) // ignore ray-casts that hit the user. DR
            {
                if (ignoreTag.Length > 0)
                {
                    for (int i = 0; i < ignoreTag.Length; i++)
                    {
                        if (hit.transform.CompareTag(ignoreTag[i]))
                        {
                            return;
                        }
                    }
                }
                wantedPosition.x = hit.point.x;
                wantedPosition.z = hit.point.z;
                wantedPosition.y = Mathf.Lerp(hit.point.y + bumperCameraHeight, wantedPosition.y, Time.deltaTime * damping);
            }

            transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

            Vector3 lookPosition = target.TransformPoint(targetLookAtOffset);

            if (smoothRotation)
            {
                Quaternion wantedRotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);
            }
        }
    }
}
