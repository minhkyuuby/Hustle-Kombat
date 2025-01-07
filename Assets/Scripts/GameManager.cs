using DG.Tweening;
using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = new GameManager();

    public static System.Action<bool> OnFinishAction;

    public bool isStarted = false;
    public bool isFinished = false;

    [Header("Scene References")]
    public GameObject MenuScreen;
    public FinishScreen FinishedScreen;

    public GameObject StartTxt;

    [Space]
    public PlayerInput inputHandler;
    public BehaviorGraphAgent behaviorAgent;

    private void Awake()
    {
        instance = this;
        inputHandler.enabled = false;
        behaviorAgent.enabled = false;
        MenuScreen.SetActive(true);
        StartTxt.SetActive(false);
    }

    public void StartGame()
    {
        if(isStarted) { return; }
        isFinished = false;
        isStarted = true;
        MenuScreen.SetActive(false);
        DOVirtual.DelayedCall(2f, () =>
        {
            inputHandler.enabled = true;
            behaviorAgent.enabled = true;
        });

        StartTxt.transform.localScale = Vector3.one * 0.5f;
        StartTxt.SetActive(true);
        StartTxt.transform.DOScale(1.2f, 0.2f).SetDelay(1.5f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.2f, () =>
            {
                StartTxt.SetActive(false);
            });
        });

    }

    public void FinishGame(bool won = true)
    {
        if (isFinished) return;
        isFinished = true;
        inputHandler.enabled = false;
        behaviorAgent.enabled = false;

        OnFinishAction?.Invoke(won);
        DOVirtual.DelayedCall(3f, () =>
        {
            FinishedScreen.ShowFinish(won);
        });
    }

    public void RefreshScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
