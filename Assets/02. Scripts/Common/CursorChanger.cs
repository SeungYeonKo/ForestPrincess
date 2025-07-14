using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D clickedCursor;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        Cursor.SetCursor(defaultCursor, hotspot, cursorMode);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.SetCursor(clickedCursor, hotspot, cursorMode);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.SetCursor(defaultCursor, hotspot, cursorMode);
        }
    }
}
