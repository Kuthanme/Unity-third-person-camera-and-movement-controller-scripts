//Reference script:https://www.youtube.com/watch?v=LbDQHv9z-F0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public float minDistance = 0.1f;
    public float maxDistance = 1.33f;
    public float smooth = 10.0f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    // Use this for initialization
    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;
        
        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit) && !hit.collider.isTrigger)
            distance = Mathf.Clamp((hit.distance * 0.87f), minDistance, maxDistance);//
        else
            distance = maxDistance;

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}

