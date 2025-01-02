using System;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public Action<float, float, float> OnStatChange;

    float healthPoint = 100;
    float stamina = 100;
    float aura = 10;

    public int healthPointMax = 100;
    public int staminaMax = 100;
    public int auraMax = 10;

    public float stamniaFillrate = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitValue();
    }

    private void FixedUpdate()
    {
        StaminaRefill();
    }

    public void InitValue()
    {
        healthPoint = healthPointMax;
        stamina = staminaMax;
        aura = auraMax;
        OnStatChangeInVoke();
    }

    public bool LoseHeath(float amount)
    {
        healthPoint -= amount;
        if(healthPoint < 0) healthPoint = 0;

        OnStatChangeInVoke();
        return healthPoint > 0f;
    }

    public bool CostStamina(int amount)
    {
        bool outOfSta = stamina < amount;

        stamina -= amount;
        stamina = Math.Clamp(stamina, 0f, staminaMax);
        OnStatChangeInVoke();
        return !outOfSta;
    }

    bool boostStaminaFillrate = false;
    void StaminaRefill() // auto refill stamina
    {
        if(stamina <= 0f && !boostStaminaFillrate) // out of stamina, boost up fill rate
        {
            stamniaFillrate *= 2;
            boostStaminaFillrate = true;
        } else if(stamina >= staminaMax/2f && boostStaminaFillrate)
        {
            stamniaFillrate /= 2;
            boostStaminaFillrate = false;
        }


        if(stamina < staminaMax)
        {
            stamina += stamniaFillrate * Time.fixedDeltaTime;
            stamina = Math.Clamp(stamina, 0, staminaMax);
            OnStatChangeInVoke();
        }


    }

    public bool CostAura(int amount)
    {
        if (aura < amount)
            return false;

        aura -= amount;
        OnStatChangeInVoke();
        return true;
    }

    public void GainAura(int amount)
    {
        aura += amount;
        aura += Math.Clamp(aura, 0, auraMax);
        OnStatChangeInVoke();
    }

    void OnStatChangeInVoke()
    {
        OnStatChange?.Invoke(healthPoint/ healthPointMax, stamina / staminaMax, aura / auraMax);
    }
}
