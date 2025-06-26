using UnityEngine;

namespace LeoLuz.PlugAndPlayJoystick
{
    public class PlayerMove : MonoBehaviour
    {
        public string HorizontalAxis = "Horizontal";
        public string VerticalAxis = "Vertical";
        public float speed = 5f;

        // 회전 속도
        public float rotationSpeed = 10f; 

        private Rigidbody rb;
        private Animator animator;

        // Update에서 사용할 입력값 저장 변수
        private Vector3 inputVector;
        // 마지막으로 입력된 (회전할) 방향 저장
        private Vector3 lastMoveDirection;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            // X, Z축 회전을 고정하여 쓰러지는 오류 해결
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

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
            float horizontalInput = Input.GetAxis(HorizontalAxis);
            float verticalInput = Input.GetAxis(VerticalAxis);
            inputVector = new Vector3(horizontalInput, 0f, verticalInput);

            // 마지막 이동 방향 업데이트
            if (inputVector.sqrMagnitude > 0.01f)
            {
                lastMoveDirection = inputVector.normalized;
            }

            // 애니메이션용 이동 여부 처리
            bool isMoving = inputVector.sqrMagnitude > 0.01f;
            if (animator != null)
            {
                animator.SetBool("IsMove", isMoving);
            }
        }

        void FixedUpdate()
        {
            Vector3 movement = inputVector * speed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

            if (lastMoveDirection.sqrMagnitude > 0.01f)
            {
                // 입력 혹은 마지막 이동 방향을 바라보도록 회전 계산 (Y축 기준으로)
                Quaternion targetRotation = Quaternion.LookRotation(lastMoveDirection, Vector3.up);
                // 회전 부드럽게 
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
            }
        }
    }
}
