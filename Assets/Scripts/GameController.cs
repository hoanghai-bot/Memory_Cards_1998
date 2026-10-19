using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type_Cards
{
    Heart, Diamond, Club, Spade
}
public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Type_Cards[] type = { Type_Cards.Heart, Type_Cards.Diamond, Type_Cards.Spade, Type_Cards.Club };
    public GameObject[] levelPrefabs;
    public int starPoint;

    public Sprite CardBack;

    public Sprite[] Heart;
    public Sprite[] Diamond;
    public Sprite[] Club;
    public Sprite[] Spade;

    public ParticleSystem mouseClick;
    private Camera mainCamera;

    [HideInInspector] public string animExplode = "explodeSpring";
    public Animator explode1;
    public Animator explode2;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        mainCamera = Camera.main;

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;

            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

            mouseClick.transform.position = worldPos;
            mouseClick.Play();
        }
    }

    public void Shuffle()
    {
        if (MatchGame.instance == null || MatchGame.instance.isShuffling || !MatchGame.instance.canTouch) return;
        if (GetData.instance.coin >= 10)
        {
            GetData.instance.coin -= 10;
            MatchGame.instance.ReChoose();
            MatchGame.instance.ActiveOffHint();
            Observer.Notify(EventGame.CloseCards);
            MatchGame.instance.AddShuffleAndMoveCard();
        }
    }
    public void Hint()
    {
        if (MatchGame.instance == null || MatchGame.instance.isShuffling) return;
        if (!MatchGame.instance.CheckPair()) return; // Không trừ coin nếu không có cặp nào trên bàn (Lỗi 8)

        if (GetData.instance.coin >= 20)
        {
            GetData.instance.coin -= 20;
            MatchGame.instance.ActiveOnHint();
        }
    }
    public void ActiveEplode(Transform trf1,Transform trf2)
    {
        explode1.transform.position = trf1.position;
        explode2.transform.position = trf2.position;

        explode1.gameObject.SetActive(true);
        explode2.gameObject.SetActive(true);

        explode1.Play(animExplode);
        explode2.Play(animExplode);

        StartCoroutine(resetExplode());
    }
    IEnumerator resetExplode()
    {
        yield return new WaitForSeconds(.5f);
        explode1.gameObject.SetActive(false);
        explode2.gameObject.SetActive(false);
    }
}
