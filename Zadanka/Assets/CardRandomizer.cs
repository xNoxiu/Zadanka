using UnityEngine;
using System.Collections.Generic;

public class CardRandomizer : MonoBehaviour
{
    public CardList cardList;
    public float lift = 0.5f;
    public float winScale = 1.2f;

    public Vector3 playerPos = new Vector3(30f, 4f, 6f);
    public Vector3 computerPos = new Vector3(30f, 4f, 14f);

    private List<Transform> allCards = new List<Transform>();
    private Queue<Transform> deck = new Queue<Transform>();

    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();

    private Transform playerCard, computerCard;
    private Vector3 originalPlayerScale, originalComputerScale;

    void Start()
    {
        foreach (GameObject prefab in cardList.cards)
        {
            GameObject instance = GameObject.Find(prefab.name);
            if (instance != null)
            {
                Transform t = instance.transform;
                allCards.Add(t);
                originalPositions[t] = t.position;
            }
            else
            {
                Debug.LogWarning("Ale checa ni ma obiektu: " + prefab.name);
            }
        }

        ResetDeck();
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        if (playerCard != null)
        {
            playerCard.position = originalPositions[playerCard];
            playerCard.localScale = originalPlayerScale;
        }

        if (computerCard != null)
        {
            computerCard.position = originalPositions[computerCard];
            computerCard.localScale = originalComputerScale;
        }

        if (deck.Count < 2)
        {
            ResetGame();
        }

        playerCard = deck.Dequeue();
        computerCard = deck.Dequeue();

        originalPlayerScale = playerCard.localScale;
        originalComputerScale = computerCard.localScale;

        playerCard.position = playerPos + Vector3.up * lift;
        computerCard.position = computerPos + Vector3.up * lift;

        int pVal = ParseCardStrength(playerCard.name);
        int cVal = ParseCardStrength(computerCard.name);

        if (pVal > cVal)
        {
            playerCard.localScale *= winScale;
            Debug.Log("Brawo, wygra³eœ, chwalisz siê czy ¿alisz?");
        }
        else
        {
            computerCard.localScale *= winScale;
            Debug.Log("XD Komputer ciê oje- ogra³ znaczy siê.");
        }
    }

    void ResetGame()
    {
        foreach (var card in allCards)
        {
            card.position = originalPositions[card];
            card.localScale = Vector3.one;
        }

        ResetDeck();
    }

    void ResetDeck()
    {
        deck.Clear();
        foreach (var card in allCards)
        {
            deck.Enqueue(card);
        }
    }

    int ParseCardStrength(string cardName)
    {
        if (cardName.StartsWith("Card"))
        {
            string num = cardName.Substring(4);
            int.TryParse(num, out int result);
            return result;
        }
        return 0;
    }
}
