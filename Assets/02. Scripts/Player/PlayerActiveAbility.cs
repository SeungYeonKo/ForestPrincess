using UnityEngine;
using System.Collections;

public class PlayerActiveAbility : MonoBehaviour
{
    public static PlayerActiveAbility Instance { get; private set; }
    private bool isSleeping = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Sleep()
    {
        if (isSleeping) return;
        StartCoroutine(SleepRoutine());
    }

    private IEnumerator SleepRoutine()
    {
        isSleeping = true;

        PlayerAnimationAbility.Instance.TriggerSleep(true);
        Debug.Log("잠자기");

        // 8초 대기
        yield return new WaitForSeconds(8f);

        Debug.Log("잠자기 종료");
        isSleeping = false;

        PlayerAnimationAbility.Instance.TriggerIdle();

        // 끝나면 이미지 숨기기
        if (CanvasController.Instance != null)
            CanvasController.Instance.HideSleepImage();
    }
}