using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
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

    public bool IsAlive => currentHealth > 0;
    public float PercentualHealth => ((float)currentHealth) / maxHealth;

    private void Awake()
    {
        if (Entities == null) Entities = new();

        Entities.Add(Collider, this);
    }
}
