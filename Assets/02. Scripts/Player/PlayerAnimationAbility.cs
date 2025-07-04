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

    public void SetMoveState(bool isMoving)
    {
        animator.SetBool("IsMove", isMoving);
    }

    public void SetDirection(Vector2 moveDirection)
    {
        if (moveDirection.x != 0)
        {
            // flipX로 좌우 시각 반전
            spriteRenderer.flipX = moveDirection.x < 0;

            // Animator에 방향 전달
            animator.SetFloat("DirectionX", moveDirection.x);
        }
    }
}
