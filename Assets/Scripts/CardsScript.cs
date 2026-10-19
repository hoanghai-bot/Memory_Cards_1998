using DG.Tweening;
using UnityEngine;

public class CardsScript : MonoBehaviour
{
    [HideInInspector] public Sprite cardFront;
    [HideInInspector] public Sprite cardBack;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Type_Cards type;
    public int NbCasd;


    void Start()
    {
        cardBack = GameController.instance.CardBack;
        switch (type)
        {
            case Type_Cards.Heart:
                cardFront = GameController.instance.Heart[NbCasd - 1];
                break;
            case Type_Cards.Diamond:
                cardFront = GameController.instance.Diamond[NbCasd - 1];
                break;
            case Type_Cards.Spade:
                cardFront = GameController.instance.Spade[NbCasd - 1];
                break;
            case Type_Cards.Club:
                cardFront = GameController.instance.Club[NbCasd - 1];
                break;
        }
        spriteRenderer.sprite = cardBack;
    }
    public void CloseCard()
    {
        DOTween.Sequence()
            .Append(transform.DOScaleX(0, .2f))
            .AppendCallback(() => { spriteRenderer.sprite = cardBack; })
            .Append(transform.DOScaleX(CreateDesk.Instance.ScaleCard, .2f))
            .Play();
    }
    public void OpenCard()
    {
        DOTween.Sequence()
            .Append(transform.DOScaleX(0, .2f))
            .AppendCallback(()=> { spriteRenderer.sprite = cardFront; })
            .Append(transform.DOScaleX(CreateDesk.Instance.ScaleCard,.2f))
            .Play();
    }
    public void AnimWrongCard()
    {
        animator.Play("cardWrongChoose");
    }
}
