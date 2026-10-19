using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosCardScript : MonoBehaviour
{
    private Collider2D collider2D;

    public bool canTouch = false;
    public float floor;
    public CardMatchScript card;

    public Collider2D[] hit = new Collider2D[3]; // Giữ trường cũ nếu có inspector serialize
    private List<Collider2D> hitList = new List<Collider2D>();
    ContactFilter2D contactFilter;
    public LayerMask layer;

    void Start()
    {
        collider2D = GetComponent<Collider2D>();
        floor = -transform.position.z;

        layer = LayerMask.GetMask("Slot");
        contactFilter = new ContactFilter2D()
        {
            useLayerMask = true,
            useTriggers = true,
            layerMask = layer
        };

        Observer.AddObserver(EventGame.CheckPosCard, CheckActive);
    }

    void CheckActive(object[] data)
    {
        if (canTouch) return;
        if (collider2D == null) collider2D = GetComponent<Collider2D>();
        if (collider2D == null) return;

        hitList.Clear();
        int result = collider2D.OverlapCollider(contactFilter, hitList);
        if (result > 0)
        {
            for (int i = 0; i < hitList.Count; i++)
            {
                var item = hitList[i];
                if (item == null || !item.enabled || !item.gameObject.activeInHierarchy) continue;
                if (item.gameObject == gameObject) continue;

                PosCardScript otherPos = item.GetComponent<PosCardScript>();
                if (otherPos != null && otherPos.floor > floor)
                {
                    canTouch = false;
                    if (card != null)
                    {
                        card.canTouch = false;
                    }
                    return;
                }
            }
        }
        canTouch = true;
        if (card != null)
        {
            card.canTouch = true;
            card.AddCardCanTouch();
            card.cardScript.OpenCard();
        }
    }
    private void OnDestroy()
    {
        Observer.RemoveListeners(EventGame.CheckPosCard, CheckActive);
    }
}
