using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimationAbility))]
public class PlayerMoveAbility : MonoBehaviour
{
    [Tooltip("이동 속도")]
    public float speed = 5f;

    private Rigidbody2D rb;
    private PlayerAnimationAbility animationAbility;

    private Vector2 targetPosition;
    private Vector2 stage1Target;
    private bool isMoving = false;
    private bool isStage1 = false;

    // 애니메이션용 마지막 방향 저장
    private Vector2 lastDir = Vector2.right;
    // 도달·방향 전환 판정 임계치
    private const float threshold = 0.05f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        // 물리와 렌더링 사이를 부드럽게 보간
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        animationAbility = GetComponent<PlayerAnimationAbility>();
    }

    void Update()
    {
        // 우클릭으로 이동 목표 설정
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0f;
            Vector2 clickPos = worldPos;

            Collider2D hit = Physics2D.OverlapPoint(clickPos);
            if (hit != null && hit.CompareTag("Ground"))
            {
                targetPosition = clickPos;
                Vector2 current = rb.position;

                // 어느 축을 먼저 움직일지 결정
                bool horizontalFirst = Mathf.Abs(targetPosition.x - current.x)
                                     >= Mathf.Abs(targetPosition.y - current.y);

                // 1단계 목표 (L자 궤적 첫 구간)
                stage1Target = horizontalFirst
                    ? new Vector2(targetPosition.x, current.y)
                    : new Vector2(current.x, targetPosition.y);

                isStage1 = true;
                isMoving = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isMoving)
        {
            animationAbility.SetDirection(Vector2.zero);
            return;
        }

        Vector2 current = rb.position;
        Vector2 dest = isStage1 ? stage1Target : targetPosition;
        Vector2 delta = dest - current;

        // 해당 단계 축으로만 이동 (다른 축은 0)
        Vector2 dir = new Vector2(
            Mathf.Abs(delta.x) > threshold ? Mathf.Sign(delta.x) : 0f,
            Mathf.Abs(delta.y) > threshold ? Mathf.Sign(delta.y) : 0f
        );

        // MovePosition으로 물리 이동
        Vector2 nextPos = Vector2.MoveTowards(
            current,
            dest,
            speed * Time.fixedDeltaTime
        );
        rb.MovePosition(nextPos);

        // 실제 이동 벡터로 애니메이션 방향 결정, 임계치 이하면 마지막 방향 유지
        Vector2 movement = nextPos - current;
        Vector2 animDir = movement.magnitude > threshold
            ? movement.normalized
            : lastDir;
        animationAbility.SetDirection(animDir);
        if (movement.magnitude > threshold)
            lastDir = animDir;

        // 도달 판정
        if (Vector2.Distance(current, dest) < threshold)
        {
            if (isStage1)
                isStage1 = false;   // 1단계 끝 → 2단계로
            else
                isMoving = false;   // 최종 도착
        }
    }
}
