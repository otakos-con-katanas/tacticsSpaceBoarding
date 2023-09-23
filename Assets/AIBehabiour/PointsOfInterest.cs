using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class PointsOfInterest : MonoBehaviour
{
    SphereCollider Collider;
    Color color = Color.yellow.gamma;
    bool debugMode = true;
    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        if (debugMode) debugFieldOfView(Collider.radius);
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            return;
        }
        Vector3 target = (other.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, target * Collider.radius, color);
    }
    void debugFieldOfView(float viewDistance)
    {
        Debug.DrawRay(transform.position, transform.forward * viewDistance, color);

    }
}
