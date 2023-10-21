using UnityEngine;
using UnityEngine.UI;

public class WorldUI_MovementDestinationIndicator : MonoBehaviour
{
    [SerializeField] private Image movementTargetImage;
    [SerializeField] private float normalOffset;

    private void Awake()
    {
        Messages.NewPlayerMovementDestination += ShowIndicator;
        Messages.PlayerArrivedDestination += HideIndicator;
    }

    private void OnDestroy()
    {
        Messages.NewPlayerMovementDestination -= ShowIndicator;
        Messages.PlayerArrivedDestination -= HideIndicator;
    }

    private void ShowIndicator(RaycastHit hit)
    {
        movementTargetImage.enabled = true;

        movementTargetImage.transform.position = hit.point + hit.normal * normalOffset;
        movementTargetImage.transform.forward = hit.normal;
    }

    private void HideIndicator()
    {
        movementTargetImage.enabled = false;
    }

}