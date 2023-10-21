using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public enum InteractionTargetType
{
    Terrain
}

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private void Start()
    {
        CursorInteractionManager.OnInteractionTargeted += TryInteract;
    }

    private bool wasWalking = false;
    public void Update()
    {
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
                navMeshAgent.SetDestination(hit.point);
                wasWalking = true;
                Messages.NewPlayerMovementDestination(hit);
                break;
            default:
                break;
        }
    }
}
