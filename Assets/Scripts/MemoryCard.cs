using UnityEngine;
using UnityEngine.UI;

public class MemoryCard : MonoBehaviour
{
    [Header("Card Data")]
    public string pairKey;

    [Header("Sprites")]
    public Sprite frontSprite;
    public Sprite backSprite;

    private Image cardImage;
    private Button cardButton;
    private MemoryGameManager gameManager;

    public bool IsRevealed { get; private set; }
    public bool IsMatched { get; private set; }

    private void Awake()
    {
        cardImage = GetComponent<Image>();
        cardButton = GetComponent<Button>();

        if (cardButton != null)
        {
            cardButton.onClick.AddListener(OnCardClicked);
        }
    }

    public void Initialize(MemoryGameManager manager, string newPairKey, Sprite newFrontSprite, Sprite newBackSprite)
    {
        gameManager = manager;
        pairKey = newPairKey;
        frontSprite = newFrontSprite;
        backSprite = newBackSprite;

        IsRevealed = false;
        IsMatched = false;

        ShowBack();
        SetInteractable(true);
    }

    private void OnCardClicked()
    {
        if (gameManager != null)
        {
            gameManager.CardClicked(this);
        }
    }

    public void ShowFront()
    {
        if (cardImage != null && frontSprite != null)
        {
            cardImage.sprite = frontSprite;
        }

        IsRevealed = true;
    }

    public void ShowBack()
    {
        if (cardImage != null && backSprite != null)
        {
            cardImage.sprite = backSprite;
        }

        IsRevealed = false;
    }

    public void MarkAsMatched()
    {
        IsMatched = true;
        IsRevealed = true;

        SetInteractable(false);
    }

    public void SetInteractable(bool value)
    {
        if (cardButton != null)
        {
            cardButton.interactable = value;
        }
    }
}