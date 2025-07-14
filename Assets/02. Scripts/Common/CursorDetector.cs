using UnityEngine;

public class CursorDetector : MonoBehaviour
{
    void Update()
    {
        // 1. 마우스 월드 좌표
        Vector3 wp3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        wp3.z = 0f;
        Vector2 wp2 = wp3;

        // 2. 해당 지점에 겹친 모든 콜라이더 검색
        Collider2D[] hits = Physics2D.OverlapPointAll(wp2);

        // 3. 원하는 순서대로 분기 처리
        bool hovered = false;
        foreach (var hit in hits)
        {
            if (hit.CompareTag("SleepArea"))
            {
                CursorManager.Instance.SetHoverCursor(CursorType.Sleep);
                hovered = true;
                break;   // SleepArea 우선 감지
            }
            // 다른 영역들 처리 가능
            // else if (hit.CompareTag("FishingArea")) { … }
        }

        // 4. 아무것도 없으면 기본으로 리셋
        if (!hovered)
            CursorManager.Instance.ResetHoverCursor();
    }
}
