using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    [Tooltip("이동 속도")]
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMovingToTarget = false;

    private PlayerAnimationAbility _animationAbility;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D 컴포넌트 없음");
        else
            rb.freezeRotation = true;

        _animationAbility = GetComponent<PlayerAnimationAbility>();
        if (_animationAbility == null)
            Debug.LogError("PlayerAnimationAbility 컴포넌트 없음");
    }

    void Update()
    {
        // 플레이어 마우스 이동
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos2D);
            if (hitCollider != null && hitCollider.CompareTag("Ground"))
            {
                targetPosition = mousePos2D;
                isMovingToTarget = true;
            }
        }

        // 애니메이션
        if (_animationAbility != null)
            _animationAbility.SetMoveState(isMovingToTarget);
    }

    void FixedUpdate()
    {
        if (isMovingToTarget)
        {
            Vector2 direction = (targetPosition - rb.position).normalized;
            rb.linearVelocity = direction * speed;

            if (_animationAbility != null)
                _animationAbility.SetDirection(direction);

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
