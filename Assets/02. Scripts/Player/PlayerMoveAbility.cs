using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    [Tooltip("이동 속도")]
    public float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 targetPosition;
    private bool isMovingToTarget = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D 컴포넌트 없음");
        else
            rb.freezeRotation = true;

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogError("Animator 컴포넌트 없음");
        else
            animator.applyRootMotion = false;
    }

  
     void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f;
                Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

                Collider2D hitCollider = Physics2D.OverlapPoint(mousePos2D);
                if (hitCollider != null)
                {
                    if (hitCollider.CompareTag("Ground"))
                    {
                        targetPosition = mousePos2D;
                        isMovingToTarget = true;
                    }
                    else if (hitCollider.CompareTag("NPC"))
                    {
                        Debug.Log("NPC 클릭됨 - 상호작용 로직으로 전환 가능");
                    }
                    else
                    {
                        Debug.Log("이동 불가한 영역 클릭");
                    }
                }
                else
                {
                    Debug.Log("Collider가 없는 빈 공간 클릭됨");
                }
            }

            bool isMoving = isMovingToTarget;
            if (animator != null)
                animator.SetBool("IsMove", isMoving);
        }

    

    void FixedUpdate()
    {
        if (isMovingToTarget)
        {
            Vector2 direction = (targetPosition - rb.position).normalized;
            rb.linearVelocity = direction * speed;

            if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
            {
                rb.linearVelocity = Vector2.zero;
                isMovingToTarget = false;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
