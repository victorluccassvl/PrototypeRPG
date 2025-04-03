using System;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public enum ActivationType
    {
        Passive,
        Targeted,
        Area,
        Auto,
    }

    [Flags]
    public enum ActivationBlockedBy
    {
        None,
        InvalidActivationType,
        InvalidTarget,
        OutOfRange,
        OutOfMana
    }

    [SerializeField] protected Entity owner;
    [SerializeField] protected Effect effect;
    [SerializeField] protected ActivationType activationType;
    [SerializeField] protected float requiredMana;
    [SerializeField] protected float defaultInternalCooldown;
    [SerializeField] protected float defaultGlobalCooldown;
    [SerializeField] protected float range;
    [SerializeField] protected float area;

    public abstract ActivationBlockedBy Activate();
    public abstract ActivationBlockedBy Activate(Entity target);
    public abstract ActivationBlockedBy Activate(Vector3 position);

    public bool CanActivate() => CheckActivationRequirements() == ActivationBlockedBy.None;
    public virtual ActivationBlockedBy CheckActivationRequirements()
    {
        if (FailsActivationTypeRequirement(ActivationType.Passive) && FailsActivationTypeRequirement(ActivationType.Auto)) return ActivationBlockedBy.InvalidActivationType;
        if (FailsManaRequirement()) return ActivationBlockedBy.OutOfMana;

        return ActivationBlockedBy.None;
    }

    public bool CanActivate(Entity target) => CheckActivationRequirements(target) == ActivationBlockedBy.None;
    public virtual ActivationBlockedBy CheckActivationRequirements(Entity target)
    {
        if (FailsActivationTypeRequirement(ActivationType.Targeted)) return ActivationBlockedBy.InvalidActivationType;
        if (target == null || FailsEffectHostilityType(target)) return ActivationBlockedBy.InvalidTarget;
        if (FailsManaRequirement()) return ActivationBlockedBy.OutOfMana;

        Vector2 ownerPosition = new Vector2(owner.transform.position.x, owner.transform.position.z);
        Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.z);
        if (FailsRangeRequirement(ownerPosition, targetPosition)) return ActivationBlockedBy.OutOfRange;

        return ActivationBlockedBy.None;
    }

    public bool CanActivate(Vector3 position) => CheckActivationRequirements(position) == ActivationBlockedBy.None;
    public virtual ActivationBlockedBy CheckActivationRequirements(Vector3 position)
    {
        if (FailsActivationTypeRequirement(ActivationType.Area)) return ActivationBlockedBy.InvalidActivationType;
        if (FailsManaRequirement()) return ActivationBlockedBy.OutOfMana;

        Vector2 ownerPosition = new Vector2(owner.transform.position.x, owner.transform.position.z);
        Vector2 targetPosition = new Vector2(position.x, position.z);
        if (FailsRangeRequirement(ownerPosition, targetPosition)) return ActivationBlockedBy.OutOfRange;

        return ActivationBlockedBy.None;
    }

    public bool FailsActivationTypeRequirement(ActivationType activationType) => activationType != this.activationType;
    public bool FailsManaRequirement() => requiredMana > owner.currentMana;
    public bool FailsRangeRequirement(Vector2 origin, Vector2 target) => Vector2.Distance(origin, target) > range;
    public bool FailsEffectHostilityType(Entity target) => (target.hostilityGroup != owner.hostilityGroup && effect.CanApplyTo == Effect.HostilityType.Allies) || (target.hostilityGroup == owner.hostilityGroup && effect.CanApplyTo == Effect.HostilityType.Enemies);
}