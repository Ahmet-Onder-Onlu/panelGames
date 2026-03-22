using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSpriteSwap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public Sprite normalSprite;
    public Sprite pressedSprite;

    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonImage != null && pressedSprite != null)
        {
            buttonImage.sprite = pressedSprite;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }
    }
}