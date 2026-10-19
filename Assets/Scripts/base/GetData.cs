using TMPro;
using UnityEngine;

public class GetData : MonoBehaviour
{
    public static GetData instance;

    private int _levelPlaying;
    public int levelPlaying
    {
        get => _levelPlaying;
        set
        {
            _levelPlaying = value;
            PlayerPrefs.SetInt("levelPlaying", levelPlaying);
        }
    }
    private int _levelPlayed;
    public int levelPlayed
    {
        get => _levelPlayed;
        set
        {
            _levelPlayed = value;
            PlayerPrefs.SetInt("levelPlayed", levelPlayed);
        }
    }
    private int _coin;
    public int coin
    {
        get => _coin;
        set
        {
            _coin = value;
            PlayerPrefs.SetInt("coin", coin);
            ShowCoin();
        }
    }
    [HideInInspector] public int[] starPoints;

    public TextMeshProUGUI[] textCoin;
    public TextMeshProUGUI nbHint;
    public TextMeshProUGUI nbShuffle;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        levelPlaying = PlayerPrefs.GetInt("levelPlaying", 0);
        levelPlayed = PlayerPrefs.GetInt("levelPlayed", 0);
        coin = PlayerPrefs.GetInt("coin", 100);
        Observer.AddObserver(EventGame.WhenGameWin, WhenGameWin);

        if (SceneButton.instance.haveStar)
        {
            GetArray();
        }
    }
    void WhenGameWin(object[] data)
    {
        if (levelPlaying + 1 > levelPlayed)
        {
            levelPlayed++;
        }
        if (SceneButton.instance.haveStar)
        {
            int temp = GameController.instance.starPoint;
            if (temp > starPoints[levelPlaying])
            {
                starPoints[levelPlaying] = temp;
            }
            SetArray();
        }
    }

    string SetupListStarPoints()
    {
        if (SceneButton.instance.irregular)
        {
            int[] temp = new int[SceneButton.instance.posButtons.Length];
            string starPointsString = string.Join(",", temp);
            PlayerPrefs.SetString("starPoints", starPointsString);
            return starPointsString;
        }
        else
        {
            int[] temp = new int[SceneButton.instance.amountOfLevel];
            string starPointsString = string.Join(",", temp);
            PlayerPrefs.SetString("starPoints", starPointsString);
            return starPointsString;
        }
    }
    public void SetArray()
    {
        string scoresString = string.Join(",", starPoints);
        PlayerPrefs.SetString("starPoints", scoresString);
    }
    public void GetArray()
    {
        if (!PlayerPrefs.HasKey("starPoints"))
        {
            SetupListStarPoints();
        }
        string tempS = PlayerPrefs.GetString("starPoints");
        string[] starPointStrings = tempS.Split(',');

        starPoints = new int[starPointStrings.Length];
        for (int i = 0; i < starPointStrings.Length; i++)
        {
            starPoints[i] = int.Parse(starPointStrings[i]);
        }
    }
    public void ShowCoin()
    {
        foreach(var item in textCoin)
        {
            item.text = coin.ToString();
        }
        nbHint.text = (coin/20).ToString();
        nbShuffle.text = (coin / 10).ToString();
    }
}
