using UnityEngine;

public class CardReader : MonoBehaviour
{
    public CardDataSO1 cardData;

    void Start()
    {
        if (cardData != null)
        {
            Debug.Log("== Wczytano dane karty ==");
            Debug.Log("Og�rek: " + cardData.ogorek);
            Debug.Log("Pomidor: " + cardData.pomidor);
            Debug.Log("Cebula: " + cardData.cebula);
            Debug.Log("Sa�ata (opis): " + cardData.salata);

            // Wywo�anie funkcji z SO
            cardData.SayHello();
        }
        else
        {
            Debug.LogWarning("CardData nie jest przypisany!");
        }
    }
}
