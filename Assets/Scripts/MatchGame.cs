using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MatchGame : MonoBehaviour
{
    public static MatchGame instance;

    public List<CardsScript> listSample = new List<CardsScript>();

    public Queue<CardsScript> QueueCards = new Queue<CardsScript>();
    public List<CardsScript> listCards = new List<CardsScript>();

    public List<PosCardScript> listPosCardsChild = new List<PosCardScript>();

    public List<CardsScript> cardsCanTouch = new List<CardsScript>();
    public CardsScript cardHint1;
    public CardsScript cardHint2;
    public bool shuffle = false;

    public CardsScript chooseCard1;
    public CardsScript chooseCard2;

    public Transform posCards;
    public GameObject edgingChoose;

    public SpriteRenderer hint1;
    public SpriteRenderer hint2;

    private int pairNb;
    private bool setup = true;

    public bool canTouch = true;
    public bool isShuffling = false;
    private int shuffleRetryCount = 0;
    private const int MaxShuffleRetries = 5;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        foreach (Transform pos in posCards) 
        {
            Destroy(pos.GetComponent<SpriteRenderer>());
            listPosCardsChild.Add(pos.GetComponent<PosCardScript>());
        }
        pairNb = listPosCardsChild.Count / 2;
        listSample = CreateDesk.Instance.listCards;
        

        Observer.AddObserver(EventGame.AfterCreateCard, AfterCreateCard);
    }

    void AfterCreateCard(object[] data)
    {
        int nb = 26;
        while (pairNb > 0 && nb > 0 && listSample.Count >= 2)
        {
            pairNb--;
            int random1 = Random.Range(0, nb);
            int random2 = random1 + nb;
            if (random2 < listSample.Count && random1 < listSample.Count)
            {
                listCards.Add(listSample[random1]);
                listCards.Add(listSample[random2]);
                listSample.RemoveAt(random2);
                listSample.RemoveAt(random1);
            }
            nb--;
        }

        // Tối ưu Lỗi 9: Tiêu hủy toàn bộ các thẻ bài dư thừa trong listSample tại posDeck
        foreach (CardsScript unusedCard in listSample)
        {
            if (unusedCard != null)
            {
                Destroy(unusedCard.gameObject);
            }
        }
        listSample.Clear();

        Observer.Notify(EventGame.CheckPosCard);
        AddShuffleAndMoveCard();
    }
    public void AddShuffleAndMoveCard()
    {
        if (isShuffling) return;
        if (listCards.Count == 0) return; // Không xáo nếu đã hoàn thành (Lỗi 3)

        isShuffling = true;
        canTouch = false;
        ReChoose();
        ActiveOffHint();

        QueueCards.Clear(); // Tránh dồn hàng đợi (Lỗi 7)
        foreach (CardsScript item in listCards)
        {
            if (item != null)
            {
                QueueCards.Enqueue(item);
            }
        }
        if (cardsCanTouch.Count > 0)
        {
            foreach (CardsScript item in cardsCanTouch)
            {
                if (item != null)
                {
                    var matchScript = item.GetComponent<CardMatchScript>();
                    if (matchScript != null) matchScript.canTouch = false;
                }
            }
            cardsCanTouch.Clear();
        }
        ShuffleQueue(ref QueueCards);
        StartCoroutine(waitAfterCreateCard());
    }
    IEnumerator waitAfterCreateCard()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (PosCardScript item in listPosCardsChild)
        {
            if (item == null) continue;
            if (QueueCards.Count == 0) break; // Tránh Dequeue rỗng (Lỗi 7)

            //arrange
            CardsScript card = QueueCards.Dequeue();
            if (card == null) continue;

            card.transform.DOMove(item.transform.position + new Vector3(0,0,-1), .1f);
            yield return card.transform.DORotate(item.transform.rotation.eulerAngles, .1f);

            //add in game
            if (setup)
            {
                var matchScript = card.GetComponent<CardMatchScript>();
                if (matchScript == null) matchScript = card.AddComponent<CardMatchScript>();

                var col = card.GetComponent<BoxCollider2D>();
                if (col == null) col = card.AddComponent<BoxCollider2D>();
                col.size = new Vector2(4f, 5.8f);
            }

            var cardMatch = card.GetComponent<CardMatchScript>();
            if (cardMatch != null)
            {
                cardMatch.posScript = item;
                item.card = cardMatch;
                
                if (item.canTouch)
                {
                    cardMatch.canTouch = true;
                    if (!cardsCanTouch.Contains(card))
                    {
                        cardsCanTouch.Add(card); 
                    }
                }
                else
                {
                    cardMatch.canTouch = false;
                }
            }
        }
        setup = false;
        
        // Kiểm tra điều kiện thắng trước tiên (Lỗi 3)
        if (CheckComple())
        {
            isShuffling = false;
            canTouch = true;
            Observer.Notify(EventGame.WhenGameWin);
            yield break;
        }

        if (CheckPair())
        {
            shuffleRetryCount = 0;
            yield return new WaitForSeconds(.1f);
            Observer.Notify(EventGame.OpenCards);
            isShuffling = false;
            canTouch = true;
        }
        else
        {
            // Chỉ thử xáo lại nếu có ít nhất 2 ô mở và chưa vượt quá số lần thử tối đa
            if (cardsCanTouch.Count >= 2 && shuffleRetryCount < MaxShuffleRetries)
            {
                shuffleRetryCount++;
                isShuffling = false;
                AddShuffleAndMoveCard();
            }
            else
            {
                shuffleRetryCount = 0;
                yield return new WaitForSeconds(.1f);
                Observer.Notify(EventGame.OpenCards);
                isShuffling = false;
                canTouch = true;
            }
        }
    }
    public bool CheckPair()
    {
        for (int i = 0; i < cardsCanTouch.Count; i++)
        {
            if (cardsCanTouch[i] == null) continue;
            for (int j = i + 1; j < cardsCanTouch.Count; j++)
            {
                if (cardsCanTouch[j] == null) continue;
                if (cardsCanTouch[i].NbCasd == cardsCanTouch[j].NbCasd)
                {
                    cardHint1 = cardsCanTouch[i];
                    cardHint2 = cardsCanTouch[j];
                    return true;
                }
            }
        }
        cardHint1 = null;
        cardHint2 = null;
        return false;
    }
    public void ReChoose()
    {
        chooseCard1 = null;
        if (edgingChoose != null) edgingChoose.SetActive(false);
        ActiveOffHint();
    }
    public void ActiveOnHint()
    {
        if (cardHint1 == null || cardHint2 == null)
        {
            if (!CheckPair()) return;
        }
        if (hint1 != null && hint2 != null && cardHint1 != null && cardHint2 != null)
        {
            hint1.transform.position = cardHint1.transform.position;
            hint2.transform.position = cardHint2.transform.position;
            hint1.enabled = true;
            hint2.enabled = true;
        }
    }
    public void ActiveOffHint()
    {
        if (hint1 != null) hint1.enabled = false;
        if (hint2 != null) hint2.enabled = false;
    }
    void ShuffleQueue(ref Queue<CardsScript> queue)
    {
        List<CardsScript> list = new List<CardsScript>(queue);
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            CardsScript temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        queue = new Queue<CardsScript>(list);
    }
    public void AddChoose1(CardsScript temp)
    {
        chooseCard1 = temp;
        
        if (edgingChoose != null && chooseCard1 != null)
        {
            edgingChoose.transform.position = chooseCard1.transform.position;
            edgingChoose.SetActive(true);
        }
    }
    public void AddChoose2(CardsScript temp)
    {
        chooseCard2 = temp;
        if (edgingChoose != null) edgingChoose.SetActive(false);
        StartCoroutine(CheckMatch());
    }
    public void ResetChoose1()
    {
        chooseCard1 = null;
        if (edgingChoose != null) edgingChoose.SetActive(false);
        ActiveOffHint();
    }
    IEnumerator CheckMatch()
    {
        if (chooseCard1 == null || chooseCard2 == null)
        {
            chooseCard1 = null;
            chooseCard2 = null;
            if (!isShuffling) canTouch = true;
            yield break;
        }

        ActiveOffHint();

        if (chooseCard1.NbCasd == chooseCard2.NbCasd) 
        {
            chooseCard1.animator.Play("cardRightChoose");
            chooseCard2.animator.Play("cardRightChoose");

            GameController.instance.ActiveEplode(chooseCard1.transform, chooseCard2.transform);

            var c1 = chooseCard1;
            var c2 = chooseCard2;
            var pos1 = c1.GetComponent<CardMatchScript>()?.posScript;
            var pos2 = c2.GetComponent<CardMatchScript>()?.posScript;

            if (pos1 != null) listPosCardsChild.Remove(pos1);
            if (pos2 != null) listPosCardsChild.Remove(pos2);
            cardsCanTouch.Remove(c1);
            cardsCanTouch.Remove(c2);
            listCards.Remove(c1);
            listCards.Remove(c2);

            // Vô hiệu hóa collider và ẩn GameObject ngay để Physics không quét trúng (Lỗi 2)
            if (pos1 != null)
            {
                var col = pos1.GetComponent<Collider2D>();
                if (col != null) col.enabled = false;
                pos1.gameObject.SetActive(false);
                Destroy(pos1.gameObject);
            }
            if (pos2 != null)
            {
                var col = pos2.GetComponent<Collider2D>();
                if (col != null) col.enabled = false;
                pos2.gameObject.SetActive(false);
                Destroy(pos2.gameObject);
            }

            var colCard1 = c1.GetComponent<Collider2D>();
            if (colCard1 != null) colCard1.enabled = false;
            var colCard2 = c2.GetComponent<Collider2D>();
            if (colCard2 != null) colCard2.enabled = false;

            Destroy(c1.gameObject, .5f);
            Destroy(c2.gameObject, .5f);

            yield return new WaitForEndOfFrame();

            Observer.Notify(EventGame.CheckPosCard);

            // Kiểm tra chiến thắng trước (Lỗi 3)
            if (CheckComple())
            {
                Observer.Notify(EventGame.WhenGameWin);
            }
            else
            {
                if (CheckPair())
                {
                    // Có cặp hợp lệ sẵn
                }
                else
                {
                    Observer.Notify(EventGame.CloseCards);
                    AddShuffleAndMoveCard();
                }
            }
        }
        else
        {
            chooseCard1.animator.Play("cardWrongChoose");
            chooseCard2.animator.Play("cardWrongChoose");
            yield return new WaitForSeconds(0.4f);
        }

        chooseCard1 = null;
        chooseCard2 = null; 
        if (!isShuffling)
        {
            canTouch = true; // Phục hồi khả năng click (Lỗi 1)
        }
    }
    bool CheckComple()
    {
        if(listCards.Count == 0)
        {
            return true;
        }
        return false;
    }
    private void OnDestroy()
    {
        Observer.RemoveListeners(EventGame.AfterCreateCard, AfterCreateCard);
    }
}
