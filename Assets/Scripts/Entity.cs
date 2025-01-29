using UnityEngine;

public class Entity : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public bool IsAlive => currentHealth > 0;
    public float PercentualHealth => ((float)currentHealth) / maxHealth;
}
