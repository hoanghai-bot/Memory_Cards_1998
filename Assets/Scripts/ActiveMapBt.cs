using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActiveMapBt : MonoBehaviour
{
    public int value;
    public GameObject btPlay;
    public Animator lockAnim;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ClickActiveMap);
    }
    public void ClickActiveMap()
    {
        if(GetData.instance.coin >= value) 
        {
            GetData.instance.coin -= value;
            GetComponent<Button>().onClick.RemoveAllListeners();
            btPlay.SetActive(true);
            lockAnim.Play("lock");
        }
    }
}
