using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        CursorInteractionManager.OnInteractionTargeted += TryInteract;
    }

    public void TryInteract(InteractionTargetType targetType, RaycastHit hit)
    {
        switch (targetType)
        {
            case InteractionTargetType.Terrain:
                Debug.LogError("oi");
                navMeshAgent.SetDestination(hit.point);
                break;
            default:
                break;
        }
    }
}
