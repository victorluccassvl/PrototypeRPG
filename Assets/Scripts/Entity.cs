using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public static Dictionary<Collider, Entity> Entities;
    public static Entity Get(Collider collider)
    {
        if (Entities == null) return null;
        return Entities.GetValueOrDefault(collider, null);
    }

    [field: SerializeField] public Collider Collider { get; private set; }

    public int currentHealth;
    public int maxHealth;
    public int currentMana;
    public int maxMana;
    public int hostilityGroup;

    public bool IsAlive => currentHealth > 0;
    public float PercentualHealth => ((float)currentHealth) / maxHealth;

    private void Awake()
    {
        if (Entities == null) Entities = new();

        Entities.Add(Collider, this);
    }
}
