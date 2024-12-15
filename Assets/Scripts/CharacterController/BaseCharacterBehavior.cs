using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using static UnityEngine.Rendering.DebugUI;

public class BaseCharacterBehavior : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject groundChecker;

    public float moveSpeed = 5f;
    public float quickStepSpeed = 15f;
    public float jumpForce = 10f;

    #region VARIABLES
    Vector2 moveDirection = Vector2.zero;
    bool isGround = true;
    bool isQuickstepping = false;
    bool isGuarding = false;

    bool isAttacking = false;

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        isGround = groundChecker.transform.position.y <= 0.15f;
        //isAttacking = 
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).shortNameHash.ToString());

        animator.SetBool("isGround", isGround);
        handleMovement();
        handleDashing();
    }

    #region IN FIXED-UPDATE METHODS

    // Movement Handle
    private float lastTimeInputMove = -1f;
    private float timeFromMoveToStop = 0.35f;
    private float moveInputAbs = 0f;

    private float lastMoveAnimSpeed = 1f;

    int moveSide = 0; // side == 1 is right, side == -1 is left
    bool isInputMoving = false;
    void handleMovement()
    {
        if (!isGround || isQuickstepping || isGuarding || isAttacking) return;

        moveInputAbs = Mathf.Abs(moveDirection.x);

        float vecX = moveDirection.x * moveSpeed;

        if (moveInputAbs < 0.1f)
        {
            if (isInputMoving)
            {
                lastTimeInputMove = Time.time;
                lastMoveAnimSpeed = animator.GetFloat("moveAnimSpeed");
            }
            isInputMoving = false;
            animator.SetFloat("horizontalMove", 0);

            if (Time.time - lastTimeInputMove < timeFromMoveToStop && !isInputMoving)
            {
                animator.SetFloat("moveAnimSpeed", Mathf.Lerp(lastMoveAnimSpeed, 1f, (Time.time - lastTimeInputMove) / timeFromMoveToStop));
                vecX = Mathf.Lerp(moveSide, 0f, (Time.time - lastTimeInputMove) / timeFromMoveToStop) * moveSpeed;
                //animator.SetBool("isMoving", true);
            }
        } else
        {
            //animator.SetBool("isMoving", true);
            isInputMoving = true;
            moveSide = moveDirection.x > 0 ? 1 : -1;

            animator.SetFloat("horizontalMove", moveDirection.x);
            animator.SetFloat("moveAnimSpeed", Mathf.Clamp(Mathf.Abs(moveInputAbs), 0.5f, 1f));
        }

        rb.linearVelocity = new Vector2(vecX, rb.linearVelocity.y);

    }

    void handleDashing()
    {
        if (!isQuickstepping || !isGround || isAttacking) return;
        rb.linearVelocity = new Vector2(moveSide * quickStepSpeed, rb.linearVelocity.y);
    }


    #endregion

    #region PUBLIC METHODS
    public void SetMoveDirection(Vector2 value) {
        moveDirection = new Vector2(value.x, 0);
    }

    Tween quickstepTween;
    public void TriggerQuickStep(Vector3 direction)
    {
        if (!isGround || isQuickstepping || isGuarding) return;
        if (direction.x > 0f)
        {
            animator.SetTrigger("quickStepF");
            moveSide = 1;
        } else
        {
            animator.SetTrigger("quickStepB");
            moveSide = -1;
        }
        quickstepTween.Kill();
        isQuickstepping = true;
        quickstepTween = DOVirtual.DelayedCall(0.32f, () =>
        {
            isQuickstepping = false;
        });
    }

    void CancelQuickstep()
    {
        if (isQuickstepping)
        {
            quickstepTween.Kill();
            isQuickstepping = false;
        }
    }

    bool jumped = false; // handle double input problem with this var
    public void TriggerJump()
    {
        if (jumped || !isGround || isAttacking) return;
        CancelQuickstep();
        animator.SetTrigger("jump");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumped = true;
        DOVirtual.DelayedCall(1f, () =>
        {
            jumped = false;
        });
    }

    Tween guardCancelTween;
    public void PerformGuard()
    {
        if (!isGround || isQuickstepping) return;
        guardCancelTween.Kill();
        isGuarding = true;
        animator.SetBool("isGuard", true);
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public void GuardCancel()
    {
        if (!isGuarding) return;
        animator.SetBool("isGuard", false);
        guardCancelTween = DOVirtual.DelayedCall(0.1f, () =>
        {
            isGuarding = false;
        });
    }

    Tween attackTween;
    public void PerformPunchAttack()
    {
        if (!isGround || isAttacking || isQuickstepping) return;
        animator.SetTrigger("punch");
        isAttacking = true;
        attackTween.Kill();
        attackTween = DOVirtual.DelayedCall(0.32f, () =>
        {
            isAttacking = false;
        });
    }

    public void PerformKickAttack()
    {
        if (!isGround || isQuickstepping || isAttacking) return;
        animator.SetTrigger("kick");
        isAttacking = true;
        attackTween.Kill();
        attackTween = DOVirtual.DelayedCall(0.32f, () =>
        {
            isAttacking = false;
        });
    }
    #endregion
}
