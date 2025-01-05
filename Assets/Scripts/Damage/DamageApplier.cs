using UnityEngine;

public class DamageApplier : MonoBehaviour
{
    public GameObject sourceObject;
    public float damageValue;
    public bool isKnockDamage;

    public void GainAuraSourceObject(float amount)
    {
        if (sourceObject == null) return;

        if (sourceObject.TryGetComponent(out Stat stat))
        {
            stat.GainAura(amount);
        }
    }
}
