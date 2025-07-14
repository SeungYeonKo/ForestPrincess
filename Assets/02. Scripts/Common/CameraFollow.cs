using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("따라갈 대상 (플레이어)")]
    public Transform target;

    [Tooltip("카메라가 따라가는 속도")]
    public float smoothSpeed = 5f;

    [Tooltip("카메라의 z축 고정 값")]
    public float cameraZ = -10f;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, cameraZ);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
