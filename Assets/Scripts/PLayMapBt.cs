using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PLayMapBt : MonoBehaviour
{
    public Sprite BGGamePlay;
    public Sprite cardBack;
    public string explode;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ClickChangeMap);
    }
    public void ClickChangeMap()
    {
        SceneController.instance.BGGamePlay.GetComponent<Image>().sprite = BGGamePlay;
        GameController.instance.CardBack= cardBack;
        GameController.instance.animExplode = explode;
        SceneButton.instance.ClickPlay();
    }
}
