using UnityEngine;

public class PlayerAnimationAbility : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogError("Animator 컴포넌트 없음");
        else
            animator.applyRootMotion = false;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer 컴포넌트 없음");
    }


    public void SetDirection(Vector2 moveDirection)
    {
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            float absX = Mathf.Abs(moveDirection.x);
            float absY = Mathf.Abs(moveDirection.y);

            if (absY >= absX) // 위아래 우선
            {
                bool isUpMoving = moveDirection.y > 0.1f;
                bool isDownMoving = moveDirection.y < -0.1f;

                animator.SetBool("IsUpMove", isUpMoving);
                animator.SetBool("IsDownMove", isDownMoving);
                animator.SetBool("IsSideMove", false);
            }
            else // 좌우 우선
            {
                bool isSideMoving = absX > 0.1f;
                animator.SetBool("IsSideMove", isSideMoving);
                animator.SetBool("IsUpMove", false);
                animator.SetBool("IsDownMove", false);

                if (isSideMoving)
                    spriteRenderer.flipX = moveDirection.x < 0;
            }
        }
        else
        {
            animator.SetBool("IsUpMove", false);
            animator.SetBool("IsDownMove", false);
            animator.SetBool("IsSideMove", false);
            Debug.Log($"Idle 진입 조건 체크 → Up: {animator.GetBool("IsUpMove")}, Down: {animator.GetBool("IsDownMove")}, Side: {animator.GetBool("IsSideMove")}");
            Debug.Log("Idle 상태로 전환 시도");
        }
    }

}
