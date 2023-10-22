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

    private void ShowIndicator(Vector3 position, Vector3 normal)
    {
        movementTargetImage.enabled = true;

        movementTargetImage.transform.position = position + normal * normalOffset;
        movementTargetImage.transform.forward = normal;
    }

    private void HideIndicator()
    {
        movementTargetImage.enabled = false;
    }

}