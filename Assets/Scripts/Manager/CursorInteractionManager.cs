using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public enum InteractionTargetType
{
    Terrain,
    Entity,
}

public class CursorInteractionManager : SingletonMonoBehaviour<CursorInteractionManager>
{
    [SerializeField] private float maxRaycastDistance = 100f;
    [SerializeField] private LayerMask entityLayer;
    [SerializeField] private LayerMask terrainLayer;

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

        if (Physics.Raycast(ray, out hit, maxRaycastDistance, entityLayer.value + terrainLayer.value, QueryTriggerInteraction.Collide))
        {
            lastHitPoint = hit.point;
            int layerMask = 1 << hit.collider.gameObject.layer;

            if (layerMask == terrainLayer.value) OnInteractionTargeted(InteractionTargetType.Terrain, hit);
            else if (layerMask == entityLayer.value) OnInteractionTargeted(InteractionTargetType.Entity, hit);
        }
    }
}
