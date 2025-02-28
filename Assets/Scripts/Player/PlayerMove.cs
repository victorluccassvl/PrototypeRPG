using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Player owner;

    [SerializeField] private float defaultStopDistance;

    private float currentFollowStopDistance;
    private Entity followedEntity;
    private RaycastHit destinationHit;

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

    private bool wasMoving = false;
    public void Update()
    {
        if (float.IsInfinity(owner.NavMeshAgent.remainingDistance)) return;

        if (owner.NavMeshAgent.remainingDistance <= owner.NavMeshAgent.stoppingDistance && wasMoving)
        {
            wasMoving = false;
            if (followedEntity == null) Messages.PlayerArrivedTerrainDestination();
            else Messages.PlayerReachedFollowTarget();
        }
    }

    public void SetFollowDistance(float distance) => currentFollowStopDistance = distance;
    public void SetFollowTarget(Entity followedEntity)
    {
        if (followedEntity == null) return;
        if (followedEntity == owner) return;
        if (followedEntity == this.followedEntity) return;

        this.followedEntity = followedEntity;
        StopMovementCoroutines();
        owner.NavMeshAgent.stoppingDistance = currentFollowStopDistance;
        movePlayerToFollowTargetCoroutine = StartCoroutine(MovePlayerToFollowTargetRoutine());
    }

    private Coroutine movePlayerToFollowTargetCoroutine = null;
    private IEnumerator MovePlayerToFollowTargetRoutine()
    {
        owner.NavMeshAgent.SetDestination(followedEntity.transform.position);

        while (owner.NavMeshAgent.pathPending) yield return null;

        Messages.NewPlayerFollowTarget();
        wasMoving = true;
        movePlayerToFollowTargetCoroutine = null;
    }

    public void SetDestination(RaycastHit hit)
    {
        destinationHit = hit;
        followedEntity = null;
        StopMovementCoroutines();
        owner.NavMeshAgent.stoppingDistance = defaultStopDistance;
        movePlayerToDestinationCoroutine = StartCoroutine(MovePlayerToDestinationRoutine());
    }

    private Coroutine movePlayerToDestinationCoroutine = null;
    private IEnumerator MovePlayerToDestinationRoutine()
    {
        owner.NavMeshAgent.SetDestination(destinationHit.point);

        while (owner.NavMeshAgent.pathPending) yield return null;

        wasMoving = true;
        Physics.Raycast(owner.NavMeshAgent.destination + Vector3.up * 0.1f, Vector3.down, out destinationHit, 0.2f);
        Messages.NewPlayerMovementTerrainDestination(destinationHit.point, destinationHit.normal);

        movePlayerToDestinationCoroutine = null;
    }

    private void StopMovementCoroutines()
    {
        if (movePlayerToDestinationCoroutine != null)
        {
            StopCoroutine(movePlayerToDestinationCoroutine);
            movePlayerToDestinationCoroutine = null;
        }
        if (movePlayerToFollowTargetCoroutine != null)
        {
            StopCoroutine(movePlayerToFollowTargetCoroutine);
            movePlayerToFollowTargetCoroutine = null;
        }
    }
}
