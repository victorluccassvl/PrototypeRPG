using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Player owner;

    [SerializeField] private BasicAttackAbility basicAttackAbility;

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
                Entity target = Entity.Get(hit.collider);
                if (basicAttackAbility.CanActivate(target)) basicAttackAbility.Activate(target);
                owner.Move.SetFollowTarget(Entity.Get(hit.collider));
                break;
            default:
                break;
        }
    }
}
