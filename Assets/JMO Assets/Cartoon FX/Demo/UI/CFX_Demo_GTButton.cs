using UnityEngine;
using UnityEngine.UI;

public class CFX_Demo_UIButton : MonoBehaviour
{
    public Color NormalColor = new Color32(128, 128, 128, 128);
    public Color HoverColor = new Color32(128, 128, 128, 128);

    public string Callback;
    public GameObject Receiver;

    private Image buttonImage;
    private Button button;
    private RectTransform rectTransform;
    private bool isOver;

    //-------------------------------------------------------------

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();

        // Set up button click event
        button.onClick.AddListener(OnClick);
    }

    void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out localPoint);

        if (rectTransform.rect.Contains(localPoint))
        {
            if (!isOver)
            {
                isOver = true;
                buttonImage.color = HoverColor;
            }
        }
        else
        {
            if (isOver)
            {
                isOver = false;
                buttonImage.color = NormalColor;
            }
        }
    }

    //-------------------------------------------------------------

    private void OnClick()
    {
        Receiver.SendMessage(Callback);
    }
}
