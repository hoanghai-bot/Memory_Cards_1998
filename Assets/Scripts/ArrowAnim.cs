using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnim : MonoBehaviour
{
    void Start()
    {
        float x = GetComponent<RectTransform>().anchoredPosition.x;
        GetComponent<RectTransform>().DOAnchorPosX(x + 50f,.5f).SetLoops(-1,LoopType.Yoyo);
    }

}
