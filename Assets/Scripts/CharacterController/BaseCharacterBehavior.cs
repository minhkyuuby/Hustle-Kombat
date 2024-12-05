using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BaseCharacterBehavior : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Animator animator;

    public float moveSpeed = 5f;

    #region VARIABLES
    Vector2 moveDirection = Vector2.zero;
    bool isGround = true;

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        isGround = true;
        handleMovement();
    }

    #region IN FIXED-UPDATE METHODS
    private float lastTimeInputMove = -1f;
    private float timeFromMoveToStop = 0.35f;
    private float moveInputAbs = 0f;

    int moveSide = 0; // side == 1 is right, side == -1 is left
    bool isInputMoving = false;
    void handleMovement()
    {
        moveInputAbs = Mathf.Abs(moveDirection.x);

        float vecX = moveDirection.x * moveSpeed;

        if (moveInputAbs < 0.1f)
        {
            if(isInputMoving)
            {
                lastTimeInputMove = Time.time;
            }
            isInputMoving = false;
            animator.SetFloat("horizontalMove", 0);

            if (Time.time - lastTimeInputMove < timeFromMoveToStop && !isInputMoving)
            {
                animator.SetFloat("moveAnimSpeed", 1f);
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
    #endregion

    #region PUBLIC METHODS
    public void SetMoveDirection(Vector2 value) {
        moveDirection = new Vector2(value.x, 0);
    }

    public void TriggerQuickStep()
    {
        animator.SetTrigger("quickStep");
    }
    #endregion
}
