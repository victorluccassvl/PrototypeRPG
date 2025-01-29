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
                owner.Move.TrySetDestination(hit);
                break;
            default:
                break;
        }
    }
}
