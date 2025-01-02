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
    private Stat stat;

    [SerializeField]
    private GameObject groundChecker;

    public float moveSpeed = 5f;
    public float quickStepSpeed = 15f;
    public float jumpForce = 10f;
    public float hitRecoverTime = .2f;
    public float recoverTime = 1.5f;

    #region VARIABLES
    Vector2 moveDirection = Vector2.zero;
    bool isGround = true;
    bool isQuickstepping = false;
    bool isGuarding = false;

    bool isAttacking = false;

    bool isInvincible = false;

    bool isHit = false;
    bool isTired = false;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    #region IN FIXED-UPDATE METHODS
    void FixedUpdate()
    {
        isGround = groundChecker.transform.position.y <= 0.15f;
        //isAttacking = 
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).shortNameHash.ToString());

        animator.SetBool("isGround", isGround);
        handleMovement();
        handleDashing();
    }

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
        if (!isQuickstepping || !isGround || isAttacking || isTired) return;
        rb.linearVelocity = new Vector2(moveSide * quickStepSpeed, rb.linearVelocity.y);
    }


    #endregion
    Tween hitRecoverTween;

    private void OnTriggerEnter(Collider other)
    {
        if (isHit) return;

        if (other.gameObject.CompareTag("Impact"))
        {
            if (other.TryGetComponent(out DamageApplier dmg))
            {
                if (dmg.sourceObject != gameObject)
                {
                    isHit = true;
                    hitRecoverTween = DOVirtual.DelayedCall(hitRecoverTime, () =>
                    {
                        isHit = false;
                    });

                    if(stat.LoseHeath(dmg.damageValue))
                    {
                        animator.SetTrigger("hit");
                    } else
                    {
                        animator.SetTrigger("death");
                    }
                }
            };
        }
    }

    #region PUBLIC METHODS
    public void SetMoveDirection(Vector2 value) {
        if(isTired)
        {
            moveDirection = new Vector2(0, 0);
            return;
        }
        moveDirection = new Vector2(value.x, 0);
    }

    Tween quickstepTween;
    public void TriggerQuickStep(Vector3 direction)
    {
        if (!isGround || isQuickstepping || isGuarding) return;

        if (!PerformceStatminaAction(20)) return;

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
        if (!PerformceStatminaAction(15)) return;

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
        if (!PerformceStatminaAction(3)) return;

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
        if (!PerformceStatminaAction(5)) return;

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
        if (!PerformceStatminaAction(10)) return;

        animator.SetTrigger("kick");
        isAttacking = true;
        attackTween.Kill();
        attackTween = DOVirtual.DelayedCall(0.32f, () =>
        {
            isAttacking = false;
        });
    }

    public void PerformSkill()
    {
        if (!isGround || isQuickstepping || isAttacking) return;
        if (!PerformceAuraAction(30)) return;

        animator.SetTrigger("skill");
    }

    public void PerformUltimate()
    {
        if (!isGround || isQuickstepping || isAttacking) return;
        if (!PerformceAuraAction(90)) return;

        animator.SetTrigger("ultimate");
    }
    #endregion

    Tween recoverTween;
    bool PerformceStatminaAction(int amount)
    {
        if (isTired) return false; // can't performce action while being tired

        if(!stat.CostStamina(amount)) // run out of stamnia but still let character performce the final action
        {
            isTired = true;
            animator.SetBool("isTired", true);
            recoverTween = DOVirtual.DelayedCall(recoverTime, ()=>
            {
                isTired = false;
                animator.SetBool("isTired", false);
            });
        }

        return true;
    }

    bool PerformceAuraAction(int amount)
    {
        return stat.CostAura(amount);
    }
}
