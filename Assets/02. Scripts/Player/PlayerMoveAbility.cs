using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimationAbility))]
public class PlayerMoveAbility : MonoBehaviour
{
    [Tooltip("이동 속도")]
    public float speed = 5f;

    private Rigidbody2D rb;
    private PlayerAnimationAbility animationAbility;

    private Vector2 targetPosition;     // 최종 목표 위치
    private Vector2 stage1Target;       // L자 읻오 1단계 목표 위치
    private bool isMoving = false;      // 이동 중인지
    private bool isStage1 = false;      // 1단계(첫 축인지) 이동중인지 

    // 애니메이션용 마지막 방향 저장
    private Vector2 lastDir = Vector2.right;
    // 목표 지점에 충분히 가까워 졌는지 체크 최소거리
    private const float threshold = 0.05f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 회전 고정
        rb.freezeRotation = true;

        // 물리와 렌더링 사이를 부드럽게 보간
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        animationAbility = GetComponent<PlayerAnimationAbility>();
    }

    void Update()
    {
        // idle상태 아니면 이동 차단@
        if (PlayerActiveAbility.Instance != null &&
       PlayerActiveAbility.Instance.CurrentState != PlayerActiveAbility.PlayerActionState.Idle)
        {
            return;
        }
        // 마우스 우클릭으로 이동ㅎ하기
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

                // 어느 축을 먼저 움직일지 결정 : X축 이동 거리가 더 크면 수평이동 먼저
                bool horizontalFirst = Mathf.Abs(targetPosition.x - current.x)
                                     >= Mathf.Abs(targetPosition.y - current.y);

                // 1단계 목표 (L자 이동 첫 구간)
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
