using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    Button currentBtn;
    Animator animator;
    GameManager gameManager;
    public Image icon;
    public int id;
    public bool IsFaceUp { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();  
        currentBtn = GetComponent<Button>();
        currentBtn.onClick.AddListener(delegate
        {
            gameManager.OnCardClicked(this);
        });
    }

    public void InitCard(int _id, Sprite iconSprite, GameManager manager)
    {
        id = _id;
        icon.sprite = iconSprite;
        gameManager = manager;
    }

    public void FlipUp()
    {
        IsFaceUp = true;
        animator.SetBool("isFlip", true);
    }

    public void FlipDown()
    {
        IsFaceUp = false;
        animator.SetBool("isFlip", false);
    }

    public void HideMatchedCard()
    {
        currentBtn.interactable = false;

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
