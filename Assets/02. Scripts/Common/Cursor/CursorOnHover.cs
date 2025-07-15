using UnityEngine;
using UnityEngine.EventSystems;

public class CursorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CursorType hoverCursorType = CursorType.Default;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetHoverCursor(hoverCursorType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.ResetHoverCursor();
    }
}
