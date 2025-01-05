using System;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public Action<float, float, float> OnStatChange;

    public bool skillFree = false;

    [SerializeField] float healthPoint = 100;
    [SerializeField] float stamina = 100;
    [SerializeField] float aura = 100;

    public float healthPointMax = 100;
    public float staminaMax = 100;
    public float auraMax = 100;

    public float stamniaFillrate = 10;
    public float auraFillrate = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitValue();
    }

    private void FixedUpdate()
    {
        StaminaRefill();
        AuraRefill();
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
        if(skillFree) return true;
        if (aura < amount)
            return false;

        aura -= amount;
        OnStatChangeInVoke();
        return true;
    }

    void AuraRefill() // auto refill stamina
    {
        if (aura < auraMax)
        {
            aura += auraFillrate * Time.fixedDeltaTime;
            aura = Mathf.Clamp(aura, 0, auraMax);
            OnStatChangeInVoke();
        }
    }

    public void GainAura(float amount)
    {
        Debug.Log($"gain aura amount {amount}");
        aura += amount;
        aura = Mathf.Clamp(aura, 0, auraMax);
        Debug.Log($"aura after gained {aura}");
        OnStatChangeInVoke();
    }

    void OnStatChangeInVoke()
    {
        OnStatChange?.Invoke(healthPoint/ healthPointMax, stamina / staminaMax, aura / auraMax);
    }
}
