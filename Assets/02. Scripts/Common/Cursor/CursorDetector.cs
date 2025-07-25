using UnityEngine;
using UnityEngine.EventSystems;

public class CursorDetector : MonoBehaviour
{
    void Update()
    {
     
        // 1. 마우스 월드 좌표 계산
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        Vector2 mousePos = worldPos;

        // 2. 해당 지점의 모든 Collider2D 가져오기
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);

        // 3. 영역 판별 및 처리
        foreach (var hit in hits)
        {
            if (hit.CompareTag("SleepArea"))
            {
                // SleepArea 위에 있을 때
                CursorManager.Instance.SetHoverCursor(CursorType.Sleep);

                if (Input.GetMouseButtonDown(0))
                    CanvasController.Instance.ShowSleepButton();

                return; // 더 이상 다른 검사 불필요
            }
            /*else if (hit.CompareTag("ShopArea"))
            {
                CursorManager.Instance.SetHoverCursor(CursorType.Shop);
                CanvasController.Instance.HideAllActionButtons();
                return;
            }*/
        }

        // 4. 어떤 영역에도 없으면 원상 복구
        CursorManager.Instance.ResetHoverCursor();
        CanvasController.Instance.HideAllActionButtons();
    }
}
