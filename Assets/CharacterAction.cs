using UnityEngine;


[System.Serializable]
public class CharacterAction{
    public Transform bulletPrefab;
    [SerializeField] private float actionCost;
    [SerializeField] private float speed;
    public void perform(PJ pj) {
        if (!pj.isMyTurn) return;
        if (pj.actions<=0) return;

        pj.actions-=actionCost;
        var obj = GameObject.Instantiate(bulletPrefab);
        obj.transform.position = pj.transform.position;
        var projectile = obj.GetComponent<DamagingProyectile>();
        projectile.originPj = pj;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Vector3 target = pj.mira.transform.position.normalized;
        target.y = 0;
        Vector3 dir = new Vector3(pj.lookAxis.x, 0, pj.lookAxis.z);
        // rb.AddForce(target*20, ForceMode.Impulse);
        rb.AddForce( dir * speed* Time.deltaTime, ForceMode.Force);
    }
}
