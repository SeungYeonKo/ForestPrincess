using UnityEngine;
using UnityEngine.EventSystems;

public class CursorDetector : MonoBehaviour
{
    void Update()
    {
        // 1) 월드 좌표 계산
        Vector3 wp3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        wp3.z = 0f;
        Vector2 mousePos = wp3;

        // 2) 호버 감지 부분
        bool hoveringShop = false;
        bool hoveringSleep = false;
        foreach (var hit in Physics2D.OverlapPointAll(mousePos))
        {
            if (hit.CompareTag("ShopArea"))
                hoveringShop = true;
            else if (hit.CompareTag("SleepArea"))
                hoveringSleep = true;
        }

        // 3) 커서 변경
        if (hoveringShop)
            CursorManager.Instance.SetHoverCursor(CursorType.Shop);
        else if (hoveringSleep)
            CursorManager.Instance.SetHoverCursor(CursorType.Sleep);
        else
            CursorManager.Instance.ResetHoverCursor();

        // 4) 클릭 처리
        if (!Input.GetMouseButtonDown(0)) return;

        // 4-1) UI(버튼) 클릭인지 체크
        bool clickedOverUI = EventSystem.current != null
                             && EventSystem.current.IsPointerOverGameObject();

        // 4-2) 버튼 떠 있을 땐 UI 클릭만 받고 World 처리 스킵
        if (CanvasController.Instance.IsButtonOpen && clickedOverUI)
            return;

        // 4-3) UI 밖 클릭: 영역별 버튼 토글
       /* if (hoveringShop)
            CanvasController.Instance.ShowShopButton(); */  
        else if (hoveringSleep)
            CanvasController.Instance.ShowSleepButton();
        else
            CanvasController.Instance.HideAllActionButtons();
    }
}
