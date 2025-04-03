using System.Collections;
using UnityEngine;

public class BasicAttackAbility : Ability
{
    [Header("Specific Settings")]
    [SerializeField] private bool moveToTarget;
    [SerializeField] private bool autoRepeat;

    public override ActivationBlockedBy Activate() => CheckActivationRequirements();
    public override ActivationBlockedBy Activate(Vector3 position) => CheckActivationRequirements(position);
    public override ActivationBlockedBy Activate(Entity target)
    {
        ActivationBlockedBy activationBlockedBy = CheckActivationRequirements(target);

        if (abilitCoroutine != null) StopCoroutine(abilitCoroutine);
        abilitCoroutine = StartCoroutine(AbilityRoutine(target));

        return activationBlockedBy;
    }

    public override ActivationBlockedBy CheckActivationRequirements(Entity target)
    {
        if (FailsActivationTypeRequirement(ActivationType.Targeted)) return ActivationBlockedBy.InvalidActivationType;
        if (target == null || FailsEffectHostilityType(target)) return ActivationBlockedBy.InvalidTarget;
        if (FailsManaRequirement()) return ActivationBlockedBy.OutOfMana;

        if (!moveToTarget)
        {
            Vector2 ownerPosition = new Vector2(owner.transform.position.x, owner.transform.position.z);
            Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.z);
            if (FailsRangeRequirement(ownerPosition, targetPosition)) return ActivationBlockedBy.OutOfRange;
        }

        return ActivationBlockedBy.None;
    }

    private Coroutine abilitCoroutine = null;
    private IEnumerator AbilityRoutine(Entity target)
    {
        PlayerManager.Get.Player.Move.SetFollowTarget(target);

        Vector2 ownerPosition = new Vector2(owner.transform.position.x, owner.transform.position.z);
        Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.z);
        while (FailsRangeRequirement(ownerPosition, targetPosition))
        {
            ownerPosition = new Vector2(owner.transform.position.x, owner.transform.position.z);
            targetPosition = new Vector2(target.transform.position.x, target.transform.position.z);
            yield return null;
        }

        effect.Apply(target);

        if (autoRepeat) Activate(target);
    }
}
