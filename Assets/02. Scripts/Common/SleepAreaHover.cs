using UnityEngine;
using UnityEngine.EventSystems;

public class SleepAreaHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CursorType hoverCursorType = CursorType.Sleep;

    // 마우스가 콜라이더 위로 진입했을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetHoverCursor(hoverCursorType);
    }

    // 마우스가 콜라이더에서 벗어났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.ResetHoverCursor();
    }
}
