using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance { get; private set; }

    [Header("Action Buttons")]
    public Button SleepButton;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SleepButton.gameObject.SetActive(false);
    }

    void Start()
    {
        // 버튼 클릭 리스너 등록
        SleepButton.onClick.AddListener(OnSleepButtonClicked);
    }

    public Vector2 UIOffset = new Vector2(-80, -40f);
    public void ShowSleepButton()
    {
        Vector3 OffsetPos = Input.mousePosition + new Vector3(UIOffset.x, UIOffset.y, 0f);
        SleepButton.transform.position = OffsetPos;
        SleepButton.gameObject.SetActive(true);
    }


    public void HideAllActionButtons()
    {
        SleepButton.gameObject.SetActive(false);
    }

    private void OnSleepButtonClicked()
    {
        PlayerActiveAbility.Instance.Sleep();
        HideAllActionButtons();
    }
}