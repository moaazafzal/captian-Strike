using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CFX_Demo_GTToggle : MonoBehaviour
{
    public UnityEngine.Sprite NormalSprite, HoverSprite; // Ensure UnityEngine.Sprite is used
    public Color NormalColor = new Color32(128, 128, 128, 128), DisabledColor = new Color32(128, 128, 128, 48);
    public bool State = true;

    public string Callback;
    public GameObject Receiver;

    private RectTransform rectTransform;
    private bool over;
    private Text label;
    private Image image;

    //-------------------------------------------------------------

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        label = GetComponentInChildren<Text>();

        UpdateTexture();
    }

    void Update()
    {
        Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
        if (rectTransform.rect.Contains(localMousePosition))
        {
            over = true;
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
        else
        {
            over = false;
            image.color = NormalColor;
        }

        UpdateTexture();
    }

    //-------------------------------------------------------------

    private void OnClick()
    {
        State = !State;

        if (Receiver != null && !string.IsNullOrEmpty(Callback))
        {
            Receiver.SendMessage(Callback);
        }
    }

    private void UpdateTexture()
    {
        Color col = State ? NormalColor : DisabledColor;

        if (over)
        {
            image.sprite = HoverSprite; // This is correctly using UnityEngine.Sprite
        }
        else
        {
            image.sprite = NormalSprite; // This is correctly using UnityEngine.Sprite
        }

        image.color = col;

        if (label != null)
        {
            label.color = col * 1.75f;
        }
    }
}
