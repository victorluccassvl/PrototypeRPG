using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ForcedDamageEffect : Effect
{
    [SerializeField] private Entity target;
    [SerializeField] private int damage;

    public override bool Apply(Entity target = null)
    {
        target = this.target;

        if (target == null) return false;
        if (damage <= 0) return false;

        target.currentHealth -= damage;
        if (target.currentHealth <= 0)
        {
            target.currentHealth = 0;
            if (PlayerManager.Get?.Player?.Entity == target && target != null) Messages.PlayerDied();
            else Messages.EntityDied(target);
        }

        return true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ForcedDamageEffect))]
public class ForcedDamageEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Apply"))
        {
            ForcedDamageEffect forcedDamageEffect = (ForcedDamageEffect)target;
            forcedDamageEffect.Apply();
        }
    }
}
#endif