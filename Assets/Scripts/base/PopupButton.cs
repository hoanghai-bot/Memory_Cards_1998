using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupButton : MonoBehaviour
{
    [Header("--------------HTP-------------")]
    public Button htp;
    public Button exitHtp;
    [Header("--------------Win--------------")]
    public Button homeWin;
    public Button replayWin;
    public Button next;
    [Header("--------------Lost--------------")]
    public Button homeLost;
    public Button replayLost;
    public Button htpLost;

    private void Start()
    {
        AddButtonHTP();
        AddButtonPopupWin();
        AddButtomPopupLost();
    }
    //-----------------HTP--------------------------
    void AddButtonHTP()
    {
        if (htp != null) htp.onClick.AddListener(ClickHTP);
        if (exitHtp != null) exitHtp.onClick.AddListener(ClickExitHTP);
    }
    void ClickHTP()
    {
        //SceneController.instance.HTP.SetActive(true);
        SceneController.instance.HTP.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        SceneController.instance.HTP.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    void ClickExitHTP()
    {
        //SceneController.instance.HTP.SetActive(false);
        SceneController.instance.HTP.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        SceneController.instance.HTP.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    //-------------------win--------------------------
    void AddButtonPopupWin()
    {
        if (homeWin != null) homeWin.onClick.AddListener(ClickHomeWin);
        if (replayWin != null) replayWin.onClick.AddListener(ClickReplayWin);
        if (next != null) next.onClick.AddListener(ClickNext);
    }
    void ClickHomeWin()
    {
        SceneButton.instance.ActGamePlay(false);
        //SceneController.instance.Win.SetActive(false);
        SceneController.instance.Win.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        SceneController.instance.Win.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    void ClickReplayWin()
    {
        SceneButton.instance.ClickPlayLevelPlaying();
        //SceneController.instance.Win.SetActive(false);
        SceneController.instance.Win.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        SceneController.instance.Win.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    void ClickNext()
    {
        GetData.instance.levelPlaying++;
        if (GetData.instance.levelPlaying >= GameController.instance.levelPrefabs.Length)
        {
            SceneButton.instance.PlayLevelRandom();
        }
        else
        {
            SceneButton.instance.ClickPlayLevelPlaying();
        }
        //SceneController.instance.Win.SetActive(false);
        SceneController.instance.Win.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        SceneController.instance.Win.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    //-------------------Lost-------------------------
    void AddButtomPopupLost()
    {
        if (homeLost != null) homeLost.onClick.AddListener(ClickHomeLost);
        if (replayLost != null) replayLost.onClick.AddListener(ClickReplayLost);
        if (htpLost != null) htpLost.onClick.AddListener(ClickHTP);
    }
    void ClickHomeLost()
    {
        SceneButton.instance.ActGamePlay(false);
        //SceneController.instance.Lost.SetActive(false);
        SceneController.instance.Lost.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        SceneController.instance.Lost.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    void ClickReplayLost()
    {
        SceneButton.instance.ClickPlayLevelPlaying();
        //SceneController.instance.Lost.SetActive(false);
        SceneController.instance.Lost.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        SceneController.instance.Lost.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }


}
