using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class AIMovement : MonoBehaviour
{
    public LayerMask HidableLayers;
    public LOSChecker LineOfSightChecker;
    public NavMeshAgent Agent;
    [Range(-1, 1)] public float HideSensitivity = 0;

    Coroutine MovementCoroutine;
    private Collider[] Colliders = new Collider[10];

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        LineOfSightChecker.OnGainSight += HandleOnGainSight;
        LineOfSightChecker.OnLoseSight += HandleOnLoseSight;

    }
    void HandleOnGainSight(Transform Target)
    {
        if (MovementCoroutine != null)
        {
            StopCoroutine(MovementCoroutine);
        }
        MovementCoroutine = StartCoroutine(Hide(Target));
    }
    void HandleOnLoseSight(Transform Target)
    {
        if (MovementCoroutine != null)
        {
            StopCoroutine(MovementCoroutine);
        }

    }

    private IEnumerator Hide(Transform Target)
    {
        
        while (true)
        {
            int hits = Physics.OverlapSphereNonAlloc(Agent.transform.position, LineOfSightChecker.Collider.radius, Colliders, HidableLayers);

            for (int i = 0; i < hits; i++)
            {
                if (NavMesh.SamplePosition(Colliders[i].transform.position, out NavMeshHit hit, 2f, Agent.areaMask))
                {
                    if (!NavMesh.FindClosestEdge(hit.position, out hit, Agent.areaMask))
                    {
                        Debug.LogError($"Unable to find {hit.position}");
                    }
                    if (Vector3.Dot(hit.normal, (Target.position - hit.position).normalized) < HideSensitivity)
                    {
                        Agent.SetDestination(hit.position);
                        break;
                    }
                    else
                    {
                        if (NavMesh.SamplePosition(Colliders[i].transform.position - (Target.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, Agent.areaMask))
                        {
                            if (!NavMesh.FindClosestEdge(hit2.position, out hit2, Agent.areaMask))
                            {
                                Debug.LogError($"Unable to find {hit2.position}");
                            }
                            if (Vector3.Dot(hit2.normal, (Target.position - hit2.position).normalized) < HideSensitivity)
                            {
                                Agent.SetDestination(hit2.position);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Unable to find navmes near obj {Colliders[i].transform.position} - {Colliders[i].name}");
                }
            }
            yield return null;
        }
    }
}
