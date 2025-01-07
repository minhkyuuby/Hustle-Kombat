using TMPro;
using UnityEngine;

public class FinishScreen : MonoBehaviour
{
    public TextMeshProUGUI victoryTxt;
    public TextMeshProUGUI surrenderTxt;

    //private void Start()
    //{
    //    HideFinish();
    //}

    public void ShowFinish(bool victory = true)
    {
        victoryTxt.gameObject.SetActive(victory);
        surrenderTxt.gameObject.SetActive(!victory);
        gameObject.SetActive(true);
    }

    public void HideFinish()
    {
        gameObject.SetActive(false);
    }
}
