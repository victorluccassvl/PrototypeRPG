using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CursorInteractionManager : SingletonMonoBehaviour<CursorInteractionManager>
{
    [SerializeField] private float maxRaycastDistance = 100f;
    [SerializeField] private LayerMask raycastLayers;

    public static Action<InteractionTargetType, RaycastHit> OnInteractionTargeted = delegate { };

    protected void Start()
    {
        InputManager.Get.Inputs.DefaultMap.PrimaryInteraction.performed += OnPrimaryInteractionInput;
    }

    protected override void PreDestroy()
    {
        if (InputManager.Get) InputManager.Get.Inputs.DefaultMap.PrimaryInteraction.performed -= OnPrimaryInteractionInput;
    }

    private Vector3 lastHitPoint = Vector3.zero;
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(lastHitPoint, 0.2f);
    }

    private void OnPrimaryInteractionInput(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Get.Inputs.DefaultMap.Cursor.ReadValue<Vector2>());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRaycastDistance, raycastLayers, QueryTriggerInteraction.Collide))
        {
            lastHitPoint = hit.point;
            OnInteractionTargeted(InteractionTargetType.Terrain, hit);
        }
    }
}
