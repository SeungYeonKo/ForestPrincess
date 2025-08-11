using UnityEngine;
using System.Collections;

public class PlayerActiveAbility : MonoBehaviour
{
    public static PlayerActiveAbility Instance { get; private set; }
    private bool isSleeping = false;
    public Transform SleepSpot;


    // 액션 중일 때 이동안되게 하기 위한 enum추가
    public enum PlayerActionState
    {
        Idle,
        Sleeping,
        Fishing,
        Logging,
        Talking,
        Shoppping,
        Mining
    }

    // 현재 상태 idle
    public PlayerActionState CurrentState { get; private set; } = PlayerActionState.Idle;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 상태 변경(특정 액션 상태일 때 이동 제한 위함)
    private void SetState(PlayerActionState newState)
    {
        CurrentState = newState;
    }

    public void EndAction()
    {
        CurrentState = PlayerActionState.Idle;
    }


    public void Sleep()
    {
        if (CurrentState != PlayerActionState.Idle) return; // 다른 행동 중이면 무시
        StartCoroutine(SleepRoutine());
    }

    private IEnumerator SleepRoutine()
    {
        // 현재 상태 업데이트
        SetState(PlayerActionState.Sleeping);

        if (SleepSpot != null)
        {
            transform.position = SleepSpot.position;
        }

        PlayerAnimationAbility.Instance.TriggerSleep(true);
        Debug.Log("잠자기 시작");

        yield return new WaitForSeconds(8f);
        Debug.Log("잠자기 종료");

        PlayerAnimationAbility.Instance.TriggerIdle();

        EndAction();

        if (CanvasController.Instance != null)
            CanvasController.Instance.HideSleepImage();
    }
}