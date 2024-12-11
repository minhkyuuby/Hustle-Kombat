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
        if (isQuickstepping || !isGround) return;

        moveInputAbs = Mathf.Abs(moveDirection.x);

        float vecX = moveDirection.x * moveSpeed;

        if (moveInputAbs < 0.1f)
        {
            if(isInputMoving)
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
            }
        } else
        {
            isInputMoving = true;
            moveSide = moveDirection.x > 0 ? 1: -1;

            animator.SetFloat("horizontalMove", moveDirection.x);
            animator.SetFloat("moveAnimSpeed", Mathf.Clamp(Mathf.Abs(moveInputAbs), 0.5f, 1f));
        }
        
        rb.linearVelocity = new Vector2(vecX, rb.linearVelocity.y);

    }

    void handleDashing()
    {
        if (!isQuickstepping || !isGround) return;
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
        if (!isGround || isQuickstepping) return;
        if(direction.x > 0f)
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
        quickstepTween = DOVirtual.DelayedCall(0.35f, () =>
        {
            isQuickstepping = false;
        });
    }

    bool jumped = false; // handle double input problem with this var
    public void TriggerJump()
    {
        if(jumped || !isGround) return;
        if(isQuickstepping)
        {
            quickstepTween.Kill();
            isQuickstepping = false;
        }
        animator.StopPlayback();
        animator.SetTrigger("jump");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumped = true;
        DOVirtual.DelayedCall(1.2f, ()=>
        {
            jumped = false;
        });
    }

    public void HoldGuard()
    {
        animator.SetBool("isGuard", true);
    }



    #endregion
}
