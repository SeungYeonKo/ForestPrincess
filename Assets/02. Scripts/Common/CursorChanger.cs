using UnityEngine;
using System.Collections.Generic;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [Header("커서 텍스처")]
    public Texture2D DefaultCursor;
    public Texture2D ClickedCursor;
    public Texture2D SleepCursor;
    public Texture2D FishingCursor;
    public Texture2D CutTreeCursor;
    public Texture2D MiningCursor;
    public Texture2D ShopCursor;

    [Header("커서 설정")]
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private bool isRightMouseHeld = false;
    private CursorType currentCursor = CursorType.Default;
    private CursorType hoverCursor = CursorType.Default;

    private Dictionary<CursorType, Texture2D> cursorMap;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        cursorMap = new Dictionary<CursorType, Texture2D>
        {
            { CursorType.Default, DefaultCursor },
            { CursorType.Clicked, ClickedCursor },
            { CursorType.Sleep, SleepCursor },
            { CursorType.Fishing, FishingCursor },
            { CursorType.CutTree, CutTreeCursor },
            { CursorType.Mining, MiningCursor },
            { CursorType.Shop, ShopCursor }
        };
    }

    void Start()
    {
        SetCursor(CursorType.Default);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) isRightMouseHeld = true;
        if (Input.GetMouseButtonUp(1)) isRightMouseHeld = false;

        UpdateCursorVisual();
    }

    private void UpdateCursorVisual()
    {
        if (hoverCursor != CursorType.Default)
        {
            SetCursor(hoverCursor);
        }
        else if (isRightMouseHeld)
        {
            SetCursor(CursorType.Clicked);
        }
        else
        {
            SetCursor(CursorType.Default);
        }
    }

    public void SetHoverCursor(CursorType type)
    {
        hoverCursor = type;
        UpdateCursorVisual();
    }

    public void ResetHoverCursor()
    {
        hoverCursor = CursorType.Default;
        UpdateCursorVisual();
    }

    public void SetCursor(CursorType type)
    {
        if (currentCursor == type) return;

        currentCursor = type;
        if (cursorMap.TryGetValue(type, out Texture2D tex))
        {
            Cursor.SetCursor(tex, hotspot, cursorMode);
            Debug.Log("커서 변경됨: " + type);
        }
        else
        {
            Cursor.SetCursor(DefaultCursor, hotspot, cursorMode);
        }
    }
}
