using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class LOSChecker : MonoBehaviour
{
    public SphereCollider Collider;
    [Range(36, 360)] public float fov = 90f;
    public LayerMask LineOfSightLayers;
    public LayerMask obstacleLayer;
    public delegate void GainSightEvent(Transform target);
    public GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(Transform target);
    public LoseSightEvent OnLoseSight;

    private Coroutine CheckForLineOfSightCoroutine;
    [SerializeField] bool debugMode;
    const float fovDivisor = 3;


    [SerializeField] Collider parentCollider;
    public Transform col;
    [SerializeField] Color color = Color.green;
    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    /* private void OnTriggerStay(Collider other) {
        Debug.Log("pillé algo: " + other.name);
        if (CheckLineOfSight(other.transform)) {
            CheckForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(other.transform));
        }
    }
    
    private void OnTriggerExit(Collider other) {
        OnLoseSight?.Invoke(other.transform);
        if (CheckForLineOfSightCoroutine != null) {
            StopCoroutine(CheckForLineOfSightCoroutine);
        }
    }
    bool CheckLineOfSight( Transform Target ) {
        Vector3 direction = (normalizeVectorHeight(Target.transform.position) - normalizeVectorHeight(transform.position)).normalized;
        dotProduct = Vector3.Dot(transform.forward, direction);
        cosFov = Mathf.Cos(fov);
        Debug.Log("CLOS TARGET: "+Target.name);
        Debug.DrawLine(normalizeVectorHeight(transform.position), direction, Color.cyan);
        if (dotProduct>= cosFov) {
            Debug.Log("check!");
            if  (Physics.Raycast(transform.position, direction, out RaycastHit hit, Collider.radius, LineOfSightLayers)) {
                Debug.Log("ola?");
                OnGainSight?.Invoke(Target);
                return true;
            }
        }
        return false;
    }
    IEnumerator CheckForLineOfSight(Transform Target) {
        WaitForSeconds Wait = new WaitForSeconds(0.5f);
        while(!CheckLineOfSight(Target)) {
            yield return Wait;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color= Color.magenta;
        Gizmos.DrawWireSphere(transform.position, Collider.radius);
    }
    Vector3 normalizeVectorHeight(Vector3 vector) {
        vector.y=0;
        return vector;
    } */
    private void Update()
    {
        if (debugMode) debugFieldOfView(Collider.radius);
    }

    private void OnTriggerStay(Collider other)
    {
        Renderer renderer = null;
        if (debugMode)
        {
            renderer = other.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.white);
        }
        bool isInLos = checkIfIsLineOfSight(other.transform);
        if (isInLos)
        {
            bool freeLos = freeLineOfSight(other.transform);
            if (freeLos)
            {
                if (debugMode && renderer != null)
                {
                    renderer.material.SetColor("_Color", Color.magenta);
                }
                // TODO
                // OnGainSight?.Invoke(other.transform);
            }
        }
    }
    bool freeLineOfSight(Transform other)
    {
        RaycastHit hit;
        Physics.Linecast(transform.position, other.position, out hit);
        if (debugMode)
        {
            Debug.DrawLine(transform.position, other.position, Color.red);
        }
        bool isChar = other.gameObject.layer == LayerMask.NameToLayer("Character");
        bool res = other.name == hit.collider.name;
        return res && isChar;
    }
    bool checkIfIsLineOfSight(Transform other)
    {
        bool res = false;
        if (other.gameObject.layer != LayerMask.NameToLayer("Character"))
        {
            return res;
        }

        Vector3 target = (other.transform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, target);
        float cosFov = Mathf.Cos(fov / fovDivisor);
        if (dotProduct <= cosFov)
        {
            return res;
        }

        Debug.DrawRay(transform.position, target * Collider.radius, color);
        if (Physics.Raycast(transform.position, target, out RaycastHit hit, Collider.radius, LineOfSightLayers))
        {
            res = true;
        }
        return res;
    }
    void debugFieldOfView(float viewDistance)
    {
        Debug.DrawRay(transform.position, transform.forward * viewDistance, color);
        Vector3 v = Quaternion.AngleAxis(fov, Vector3.up) * transform.forward;
        Debug.DrawRay(transform.position, v * viewDistance, color);
        v = Quaternion.AngleAxis(-fov, Vector3.up) * transform.forward;
        Debug.DrawRay(transform.position, v * viewDistance, color);
    }
}

