using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMatchScript : MonoBehaviour
{
    private Collider2D collider2D;
    public CardsScript cardScript;
    public bool canTouch = false;

    public PosCardScript posScript;
    void Start()
    {
        cardScript = GetComponent<CardsScript>();
        collider2D = GetComponent<Collider2D>();


        Observer.AddObserver(EventGame.OpenCards, OpenCards);
        Observer.AddObserver(EventGame.CloseCards, CloseCards);
    }

    void OpenCards(object[] data)
    {
        if (!canTouch) return;
        cardScript.OpenCard();
    }
    void CloseCards(object[] data)
    {
        
        cardScript.CloseCard();
    }
    public void AddCardCanTouch()
    {
        if (MatchGame.instance != null && !MatchGame.instance.cardsCanTouch.Contains(cardScript))
        {
            MatchGame.instance.cardsCanTouch.Add(cardScript);
        }
    }
    private void OnMouseDown()
    {
        if (canTouch && MatchGame.instance != null && MatchGame.instance.canTouch)
        {
            MatchGame.instance.ActiveOffHint();
            if (MatchGame.instance.chooseCard1 == null)
            {
                MatchGame.instance.AddChoose1(cardScript);
            }
            else if (MatchGame.instance.chooseCard1 == cardScript)
            {
                // Click lại vào lá bài đã chọn để hủy chọn (Lỗi 4)
                MatchGame.instance.ResetChoose1();
            }
            else
            {
                MatchGame.instance.canTouch = false;
                MatchGame.instance.AddChoose2(cardScript);
            }
        }
    }
    private void OnDestroy()
    {
        Observer.RemoveListeners(EventGame.OpenCards, OpenCards);
        Observer.RemoveListeners(EventGame.CloseCards, CloseCards);
    }
}
