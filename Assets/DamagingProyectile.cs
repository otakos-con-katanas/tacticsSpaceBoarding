using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingProyectile : MonoBehaviour {
    [System.NonSerialized] public PJ originPj;
    private void OnTriggerEnter(Collider other) {
        PJ pj;
        bool isPj = other.TryGetComponent<PJ>(out pj);
        if (!pj) return;
        if (pj == originPj) return;
        if (pj.team==originPj.team) return;
        if (!pj.active) return;
        Debug.Log("apply damage");
        Destroy(gameObject);
        pj.life -= 1;
    }
}
