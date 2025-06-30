using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    [Tooltip("좌우 입력")]
    public string HorizontalAxis = "Horizontal";
    [Tooltip("상하 입력")]
    public string VerticalAxis = "Vertical";
    [Tooltip("이동 속도")]
    public float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 inputVector;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D 컴포넌트 없음");
        else
            rb.freezeRotation = true;  // Z축 회전 고정

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator 컴포넌트 없음");
        }
        else
        {
            animator.applyRootMotion = false;
        }
    }

    void Update()
    {
        float h = Input.GetAxis(HorizontalAxis);
        float v = Input.GetAxis(VerticalAxis);
        inputVector = new Vector2(h, v);

        // 애니메이션 이동 상태 설정
        bool isMoving = inputVector.sqrMagnitude > 0.01f;
        if (animator != null)
            animator.SetBool("IsMove", isMoving);
    }

    void FixedUpdate()
    {
        // 상하좌우로만 이동
        rb.linearVelocity = inputVector * speed;
    }
}
