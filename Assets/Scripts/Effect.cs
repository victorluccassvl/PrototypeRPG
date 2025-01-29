using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    protected Entity source;

    public abstract bool Apply(Entity target);
}
