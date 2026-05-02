using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class MemoryCardDefinition
{
    public string pairKey;
    public Sprite cardSprite;
}

public class MemoryGameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float timeLimit = 60f;
    public float checkDelay = 2f;

    [Header("Card Setup")]
    public MemoryCard cardPrefab;
    public Transform cardsParent;
    public Sprite cardBackSprite;
    public List<MemoryCardDefinition> cardDefinitions = new List<MemoryCardDefinition>();

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI remainingPairsText;

    [Header("End Panel")]
    public GameObject endPanel;
    public TextMeshProUGUI endMessageText;
    public Button restartButton;
    public Button mainMenuButton;

    private MemoryCard firstSelectedCard;
    private MemoryCard secondSelectedCard;

    private float remainingTime;
    private int totalPairs;
    private int matchedPairs;

    private bool canClick = true;
    private bool gameActive = false;

    private void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }

        StartNewGame();
    }

    private void Update()
    {
        if (!gameActive)
        {
            return;
        }

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            UpdateUI();
            EndGame(false);
            return;
        }

        UpdateUI();
    }

    public void StartNewGame()
    {
        ClearOldCards();

        firstSelectedCard = null;
        secondSelectedCard = null;

        matchedPairs = 0;
        totalPairs = CountTotalPairs();

        remainingTime = timeLimit;
        canClick = true;
        gameActive = true;

        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }

        CreateCards();
        UpdateUI();
    }

    private void ClearOldCards()
    {
        if (cardsParent == null)
        {
            return;
        }

        for (int i = cardsParent.childCount - 1; i >= 0; i--)
        {
            Destroy(cardsParent.GetChild(i).gameObject);
        }
    }

    private int CountTotalPairs()
    {
        List<string> uniquePairKeys = new List<string>();

        foreach (MemoryCardDefinition definition in cardDefinitions)
        {
            if (!uniquePairKeys.Contains(definition.pairKey))
            {
                uniquePairKeys.Add(definition.pairKey);
            }
        }

        return uniquePairKeys.Count;
    }

    private void CreateCards()
    {
        if (cardPrefab == null || cardsParent == null || cardBackSprite == null)
        {
            Debug.LogError("MemoryGameManager eksik alanlara sahip. CardPrefab, CardsParent veya CardBackSprite atanmadı.");
            return;
        }

        List<MemoryCardDefinition> shuffledCards = new List<MemoryCardDefinition>(cardDefinitions);
        Shuffle(shuffledCards);

        foreach (MemoryCardDefinition definition in shuffledCards)
        {
            MemoryCard newCard = Instantiate(cardPrefab, cardsParent);
            newCard.Initialize(this, definition.pairKey, definition.cardSprite, cardBackSprite);
        }
    }

    private void Shuffle(List<MemoryCardDefinition> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            MemoryCardDefinition temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void CardClicked(MemoryCard clickedCard)
    {
        if (!gameActive)
        {
            return;
        }

        if (!canClick)
        {
            return;
        }

        if (clickedCard == null)
        {
            return;
        }

        if (clickedCard.IsMatched || clickedCard.IsRevealed)
        {
            return;
        }

        clickedCard.ShowFront();

        if (firstSelectedCard == null)
        {
            firstSelectedCard = clickedCard;
            return;
        }

        secondSelectedCard = clickedCard;
        canClick = false;

        StartCoroutine(CheckSelectedCards());
    }

    private IEnumerator CheckSelectedCards()
    {
        yield return new WaitForSeconds(checkDelay);

        bool isMatch = firstSelectedCard.pairKey == secondSelectedCard.pairKey;

        if (isMatch)
        {
            firstSelectedCard.MarkAsMatched();
            secondSelectedCard.MarkAsMatched();

            matchedPairs++;

            if (matchedPairs >= totalPairs)
            {
                EndGame(true);
                yield break;
            }
        }
        else
        {
            firstSelectedCard.ShowBack();
            secondSelectedCard.ShowBack();
        }

        firstSelectedCard = null;
        secondSelectedCard = null;

        canClick = true;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            timerText.text = "Süre: " + Mathf.CeilToInt(remainingTime).ToString();
        }

        if (remainingPairsText != null)
        {
            int remainingPairs = totalPairs - matchedPairs;
            remainingPairsText.text = "Kalan: " + remainingPairs.ToString() + " Çift";
        }
    }

    private void EndGame(bool win)
    {
        gameActive = false;
        canClick = false;

        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }

        if (endMessageText != null)
        {
            if (win)
            {
                endMessageText.text = "Tebrikler!\nTüm kartları eşleştirdin.";
            }
            else
            {
                endMessageText.text = "Süre Bitti!\nTekrar dene.";
            }
        }
    }

    public void RestartGame()
    {
        StartNewGame();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}