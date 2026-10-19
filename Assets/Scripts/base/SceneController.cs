using DG.Tweening;
using UnityEngine;


public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    [Header("-------------scene------------")]
    public GameObject BGMenu;
    public GameObject Menu;
    public GameObject BGGamePlay;
    public GameObject UIGamePlay;
    public GameObject GamePlay;
    public GameObject LevelMenu;
    [Header("-------------popup------------")]
    public GameObject HTP;
    public GameObject Shop;
    public GameObject Win;
    public GameObject Lost;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Observer.AddObserver(EventGame.WhenGameWin, WhenGameWin);
        Observer.AddObserver(EventGame.WhenGameLost, WhenGameLost);
    }
    void WhenGameWin(object[] data)
    {
        //Win.SetActive(true);
        Win.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        Win.GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetData.instance.coin += 40;
    }
    void WhenGameLost(object[] data)
    {
        //Lost.SetActive(true);
        Lost.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        Lost.GetComponent<CanvasGroup>().blocksRaycasts = true ;
    }
}
