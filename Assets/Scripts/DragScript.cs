using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragScript : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private Vector3Int direction;
    private Vector2 startPosition;

    public GameObject moveBt;

    // Khi người dùng nhấn chuột/touch vào đối tượng
    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Tính vector di chuyển từ vị trí bắt đầu đến vị trí hiện tại
        Vector2 dragVector = eventData.position - startPosition;

        if (Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y))
        {
            if (dragVector.x > 0)
            {
                direction = Vector3Int.right;
                if (dragVector.x < 40)
                {
                    moveBt.GetComponent<RectTransform>().DOAnchorPosX(-200 + dragVector.x * 10, .2f);
                }
                else
                {
                    moveBt.GetComponent<RectTransform>().DOAnchorPosX(200, .2f);
                }
            }
            else
            {
                moveBt.GetComponent<RectTransform>().DOAnchorPosX(-200, .2f);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //PlayerController.instance.MovePlayer(direction, true);
        if(moveBt.GetComponent<RectTransform>().anchoredPosition.x > 0)
        {
            moveBt.GetComponent<RectTransform>().DOAnchorPosX(200, .2f);
            SceneButton.instance.ClickLevel();
            moveBt.GetComponent<RectTransform>().DOAnchorPosX(-200, .2f).SetDelay(1f);
        }
        else
        {
            moveBt.GetComponent<RectTransform>().DOAnchorPosX(-200, .2f);
        }
    }
}
