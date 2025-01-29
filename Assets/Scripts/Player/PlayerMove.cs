using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Player owner;

    private void OnDrawGizmos()
    {
        if (owner == null || owner.NavMeshAgent == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(owner.NavMeshAgent.destination, 0.3f);

        if (owner.NavMeshAgent.hasPath)
        {
            NavMeshPath path = owner.NavMeshAgent.path;

            Gizmos.color = Color.red;
            for (int i = 0; i + 1 < path.corners.Length; i++)
            {
                Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
            }
        }
    }

    private bool wasWalking = false;
    public void Update()
    {
        if (float.IsInfinity(owner.NavMeshAgent.remainingDistance)) return;

        if (owner.NavMeshAgent.remainingDistance <= owner.NavMeshAgent.stoppingDistance && wasWalking)
        {
            wasWalking = false;
            Messages.PlayerArrivedDestination();
        }
    }

    public void TrySetDestination(RaycastHit hit)
    {
        if (setPlayerMovementDestinationCoroutine != null)
        {
            StopCoroutine(setPlayerMovementDestinationCoroutine);
            setPlayerMovementDestinationCoroutine = null;
        }
        setPlayerMovementDestinationCoroutine = StartCoroutine(SetPlayerMovementDestinationRoutine(hit));
    }

    private Coroutine setPlayerMovementDestinationCoroutine = null;
    private IEnumerator SetPlayerMovementDestinationRoutine(RaycastHit hit)
    {
        owner.NavMeshAgent.SetDestination(hit.point);

        while (owner.NavMeshAgent.pathPending) yield return null;

        wasWalking = true;
        Physics.Raycast(owner.NavMeshAgent.destination + Vector3.up * 0.1f, Vector3.down, out hit, 0.2f);
        Messages.NewPlayerMovementDestination(hit.point, hit.normal);

        setPlayerMovementDestinationCoroutine = null;
    }
}
