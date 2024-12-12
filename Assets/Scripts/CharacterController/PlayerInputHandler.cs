using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private BaseCharacterBehavior characterBehavior;

    [Header("Quickstep Settings")]
    public float quickstepDistance = 2f; // Distance to move during quickstep
    public float quickstepInputCD = 0.3f; // Time window for double-tap
    public float quickstepCooldown = 0.5f; // Time window for double-tap
    public float quickstepSpeed = 10f; // Speed of the quickstep animation

    private Vector2 moveInput;
    private float lastTapTimeLeft = -1f;
    private float lastTapTimeRight = -1f;
    private float lastTimeQuickstep = -1f;
    private bool isQuickstepEnable = true;

    private bool readyStep = false;

    private Vector3 quickstepTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (characterBehavior == null)
        {
            characterBehavior = GetComponent<BaseCharacterBehavior>();
        }
    }

    //private void Update()
    //{
    //}

    public void OnMove(CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        characterBehavior.SetMoveDirection(moveInput);
        HandleQuickstep();
    }

    public void OnJump(CallbackContext context)
    {
        if(context.performed)
        {
            characterBehavior.TriggerJump();
        }
    }

    private void HandleQuickstep()
    {
        if (Time.time - lastTimeQuickstep < quickstepCooldown) return;
        if(Mathf.Abs(moveInput.x) < 0.2f) readyStep = true;
        else
        {
            // Handle D-pad or joystick left quickstep
            if (moveInput.x < -0.8f)
            {
                if (readyStep && Time.time - lastTapTimeLeft < quickstepInputCD)
                {
                    TriggerQuickstep(Vector3.left);
                }
                lastTapTimeLeft = Time.time;

                readyStep = false;
            }

            // Handle D-pad or joystick right quickstep
            if (moveInput.x > 0.8f)
            {
                if (readyStep && Time.time - lastTapTimeRight < quickstepInputCD)
                {
                    TriggerQuickstep(Vector3.right);
                }
                lastTapTimeRight = Time.time;
                
                readyStep = false;
            }

        }
    }

    private void TriggerQuickstep(Vector3 direction)
    {
        //Debug.Log("Quick step!");
        characterBehavior.TriggerQuickStep(direction);
        lastTimeQuickstep = Time.time;
    }

    public void OnAttack(CallbackContext context)
    {

    }

    public void OnHeavyAttack(CallbackContext context) { }

    public void OnBlock(CallbackContext context) { 
        if(context.performed)
        {
            characterBehavior.PerformGuard();
        } else if(context.canceled)
        {
            characterBehavior.GuardCancel();
        }
    }

    #region COROUTINES

    IEnumerator waitForSecondCallback(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    #endregion
}
