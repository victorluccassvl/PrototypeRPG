using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Player owner;

    private void Start()
    {
        CursorInteractionManager.OnInteractionTargeted += TryInteract;
    }

    private void OnDestroy()
    {
        CursorInteractionManager.OnInteractionTargeted -= TryInteract;
    }

    public void TryInteract(InteractionTargetType targetType, RaycastHit hit)
    {
        switch (targetType)
        {
            case InteractionTargetType.Terrain:
                owner.Move.SetDestination(hit);
                break;
            case InteractionTargetType.Entity:
                owner.Move.SetFollowTarget(Entity.Get(hit.collider));
                break;
            default:
                break;
        }
    }
}
