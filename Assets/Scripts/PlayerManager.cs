using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum InteractionTargetType
{
    Terrain
}

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(navMeshAgent.destination, 0.3f);

        if (navMeshAgent.hasPath)
        {
            NavMeshPath path = navMeshAgent.path;

            Gizmos.color = Color.red;
            for (int i = 0; i + 1 < path.corners.Length; i++)
            {
                Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
            }
        }
    }

    private void Start()
    {
        CursorInteractionManager.OnInteractionTargeted += TryInteract;
    }

    private bool wasWalking = false;
    public void Update()
    {
        if (float.IsInfinity(navMeshAgent.remainingDistance)) return;

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && wasWalking)
        {
            wasWalking = false;
            Messages.PlayerArrivedDestination();
        }
    }

    public void TryInteract(InteractionTargetType targetType, RaycastHit hit)
    {
        switch (targetType)
        {
            case InteractionTargetType.Terrain:
                if (setPlayerMovementDestinationCoroutine != null)
                {
                    StopCoroutine(setPlayerMovementDestinationCoroutine);
                    setPlayerMovementDestinationCoroutine = null;
                }
                setPlayerMovementDestinationCoroutine = StartCoroutine(SetPlayerMovementDestinationRoutine(hit));
                break;
            default:
                break;
        }
    }

    private Coroutine setPlayerMovementDestinationCoroutine = null;
    private IEnumerator SetPlayerMovementDestinationRoutine(RaycastHit hit)
    {
        navMeshAgent.SetDestination(hit.point);

        while (navMeshAgent.pathPending) yield return null;

        wasWalking = true;
        Physics.Raycast(navMeshAgent.destination + Vector3.up * 0.1f, Vector3.down, out hit, 0.2f);
        Messages.NewPlayerMovementDestination(hit.point, hit.normal);

        setPlayerMovementDestinationCoroutine = null;
    }
}
