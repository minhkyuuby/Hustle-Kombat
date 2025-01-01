using System;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public Action<int, int, int> OnStatChange;

    int healthPoint = 100;
    int stamina = 100;
    int aura = 10;

    public int healthPointMax = 100;
    public int staminaMax = 100;
    public int auraMax = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitValue();
    }

    public void InitValue()
    {
        healthPoint = healthPointMax;
        stamina = staminaMax;
        aura = auraMax;
    }

    public void LoseHeath(int amount)
    {
        healthPoint -= amount;
        if(healthPoint < 0) healthPoint = 0;
        OnStatChange?.Invoke(healthPoint, stamina, aura);
    }

    public bool CostStamina(int amount)
    {
        if(stamina < amount)
            return false;

        stamina -= amount;
        return true;
    }

    public bool CostAura(int amount)
    {
        if (aura < amount)
            return false;

        aura -= amount;
        return true;
    }

    public void GainAura(int amount)
    {
        aura += amount;
    }


}
