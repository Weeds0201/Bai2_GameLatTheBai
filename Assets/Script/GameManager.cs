using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Transform cardContainer;
    public GameObject cardPrefabs;
    public Sprite[] cardIcons;
    public TMP_Text timerText;
    public TMP_Text livesText;
    public int startingLives = 5;

    private CardController firstSelectedCard;
    private bool isCheckingCards;
    private bool isGameOver;
    private float remainingTime = 5 * 60f;
    private int lives;

    // Start is called before the first frame update
    void Start()
    {
        lives = startingLives;
        UpdateTimerDisplay();
        UpdateLivesDisplay();

        for(int i = 0; i < cardIcons.Length; i ++)
        {
            GameObject card1 = Instantiate(cardPrefabs, cardContainer);
            GameObject card2 = Instantiate(cardPrefabs, cardContainer);
            //
            card1.GetComponent<CardController>().InitCard(i, cardIcons[i], this);
            card2.GetComponent<CardController>().InitCard(i, cardIcons[i], this);
        }
        //shuffle
        for(int i = 0; i < 30; i ++)
        {
            int rand = Random.Range(0, cardContainer.childCount);
            cardContainer.GetChild(rand).SetAsFirstSibling();
        }

    }

    public void OnCardClicked(CardController card)
    {
        if (isGameOver || isCheckingCards || card.IsFaceUp)
        {
            return;
        }

        card.FlipUp();

        if (firstSelectedCard == null)
        {
            firstSelectedCard = card;
            return;
        }

        StartCoroutine(CheckSelectedCards(card));
    }

    private IEnumerator CheckSelectedCards(CardController secondCard)
    {
        isCheckingCards = true;
        yield return new WaitForSeconds(1f);

        if (firstSelectedCard.id == secondCard.id)
        {
            firstSelectedCard.HideMatchedCard();
            secondCard.HideMatchedCard();
        }
        else
        {
            firstSelectedCard.FlipDown();
            secondCard.FlipDown();
            lives--;
            UpdateLivesDisplay();

            if (lives <= 0)
            {
                isGameOver = true;
            }
        }

        firstSelectedCard = null;
        isCheckingCards = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            return;
        }

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            isGameOver = true;
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int totalSeconds = Mathf.CeilToInt(remainingTime);
            timerText.text = string.Format("{0:00}:{1:00}", totalSeconds / 60, totalSeconds % 60);
        }
    }

    private void UpdateLivesDisplay()
    {
        if (livesText != null)
        {
            livesText.text = lives.ToString();
        }
    }
}
