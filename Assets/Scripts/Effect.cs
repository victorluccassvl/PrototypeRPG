using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public enum HostilityType
    {
        Enemies,
        Allies,
        Both
    }

    [field: SerializeField] public HostilityType CanApplyTo { get; }
    protected Entity source;

    public abstract bool Apply(Entity target);
}
