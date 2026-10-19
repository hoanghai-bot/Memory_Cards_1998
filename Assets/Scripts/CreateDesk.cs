using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateDesk : MonoBehaviour
{
    public static CreateDesk Instance;

    public GameObject CardPrefab;
    public float ScaleCard = 1.5f;
    public List<CardsScript> listCards = new List<CardsScript>();

    public Transform posDeck;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(CreateCardAndMove());
    }
    IEnumerator CreateCardAndMove()
    {
        foreach (Type_Cards type in GameController.instance.type)
        {
            for (int i = 1; i <= 13; i++)
            {
                var clone = Instantiate(CardPrefab, transform);
                clone.GetComponent<CardsScript>().type = type;
                clone.GetComponent<CardsScript>().NbCasd = i;
                clone.transform.localScale = new Vector2(ScaleCard, ScaleCard);
                listCards.Add(clone.GetComponent<CardsScript>());
                clone.transform.DOMove(posDeck.position, 0.2f);
            }
            yield return new WaitForSeconds(0.01f);
        }
        Observer.Notify(EventGame.AfterCreateCard);
    }
}
