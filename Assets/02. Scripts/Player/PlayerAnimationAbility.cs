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

    // SetMoveState 함수는 완전히 삭제
    public void SetDirection(Vector2 moveDirection)
    {
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            bool isUpMoving = moveDirection.y > 0.1f;
            bool isDownMoving = moveDirection.y < -0.1f;

            if (isUpMoving || isDownMoving)
            {
                animator.SetBool("IsUpMove", isUpMoving);
                animator.SetBool("IsDownMove", isDownMoving);
                animator.SetBool("IsSideMove", false);
            }
            else
            {
                bool isSideMoving = Mathf.Abs(moveDirection.x) > 0.1f;
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
        }
    }
}
