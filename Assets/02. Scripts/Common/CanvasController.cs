using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance { get; private set; }

    [Header("Action Buttons")]
    public Button SleepButton;

    [Header("Sleep Indicator")]
    public Image SleepImage;            // ① Inspector에 할당

    [Header("UI Settings")]
    public Vector2 UIOffset = new Vector2(-80, -40f);

    private RectTransform canvasRect;
    private RectTransform sleepButtonRect;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        SleepButton.gameObject.SetActive(false);
        SleepImage.gameObject.SetActive(false);   // ② 시작 시 숨김
    }

    void Start()
    {
        canvasRect = GetComponent<Canvas>().transform as RectTransform;
        sleepButtonRect = SleepButton.GetComponent<RectTransform>();
        SleepButton.onClick.AddListener(OnSleepButtonClicked);
    }

    public bool IsButtonOpen => SleepButton.gameObject.activeSelf;

    public void ShowSleepButton()
    {
        if (IsButtonOpen) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            null,
            out Vector2 localPoint
        );
        sleepButtonRect.anchoredPosition = localPoint + UIOffset;
        SleepButton.gameObject.SetActive(true);
    }

    public void HideAllActionButtons()
    {
        if (!IsButtonOpen) return;
        SleepButton.gameObject.SetActive(false);
    }

    private void OnSleepButtonClicked()
    {
        ShowSleepImage();                        // ③ 이미지 켜기
        PlayerActiveAbility.Instance.Sleep();
        HideAllActionButtons();
    }

    // ④ SleepImage 제어 메서드
    public void ShowSleepImage()
    {
        SleepImage.gameObject.SetActive(true);
    }

    public void HideSleepImage()
    {
        SleepImage.gameObject.SetActive(false);
    }
}
