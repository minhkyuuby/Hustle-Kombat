using UnityEngine;
using UnityEngine.UI;

public class CharacterStatUI : MonoBehaviour
{
    [SerializeField]
    Stat characaterStat;

    [SerializeField] Image healthBarImg;
    [SerializeField] Image staminaBarImg;
    [SerializeField] Image auraBarImg;


    private void OnEnable()
    {
        characaterStat.OnStatChange += OnStatChange;
    }

    private void OnDisable()
    {
        characaterStat.OnStatChange -= OnStatChange;
    }

    void OnStatChange(float hpPercent, float staPercent, float auraPercent)
    {
        healthBarImg.fillAmount = hpPercent;
        staminaBarImg.fillAmount = staPercent;
        auraBarImg.fillAmount = auraPercent;
    }
}
