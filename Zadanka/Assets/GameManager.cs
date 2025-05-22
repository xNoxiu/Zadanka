using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Talie")]
    public PCardList playerCardList;    // prefabry PCard1…PCard8
    public ECardList enemyCardList;     // prefabry ECard1…ECard8

    [Header("Hand Positions (do rund)")]
    public Transform playerHandPos;     // „RekaGracza”
    public Transform enemyHandPos;      // „RekaKomputera”

    [Header("Deck Positions (na koniec rundy)")]
    public Transform playerDeckPos;     // Transform obiektu „Gracz”
    public Transform enemyDeckPos;      // Transform obiektu „Komputer”

    public Vector3 lift = Vector3.up * 0.5f;
    public Vector3 warOffset = Vector3.up * 1.0f;
    public float winScale = 1.2f;

    private Queue<Transform> playerDeck = new Queue<Transform>();
    private Queue<Transform> enemyDeck = new Queue<Transform>();
    private List<Transform> warPile = new List<Transform>();

    void Start()
    {
        InitDeck(playerCardList.cards, playerDeck);
        InitDeck(enemyCardList.cards, enemyDeck);
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        PlayRound();
    }

    void InitDeck(List<GameObject> prefabs, Queue<Transform> deck)
    {
        deck.Clear();
        foreach (var pref in prefabs)
        {
            var go = GameObject.Find(pref.name);
            if (go != null) deck.Enqueue(go.transform);
            else Debug.LogWarning("Brak na scenie: " + pref.name);
        }
    }

    void PlayRound()
    {
        warPile.Clear();
        ResolveBattle(playerDeck, enemyDeck, playerHandPos.position, enemyHandPos.position);
    }

    void ResolveBattle(
        Queue<Transform> pDeck, Queue<Transform> eDeck,
        Vector3 pPos, Vector3 ePos)
    {
        // Jeœli koniec kart — reset talii
        if (pDeck.Count < 1 || eDeck.Count < 1)
        {
            Debug.Log("Reset talii do stanu pocz¹tkowego");
            Start();
            return;
        }

        // Dobieramy po karcie
        var pCard = pDeck.Dequeue();
        var eCard = eDeck.Dequeue();
        warPile.Add(pCard);
        warPile.Add(eCard);

        // Ustawiamy w „rêkach”
        pCard.position = pPos + lift;
        eCard.position = ePos + lift;

        int pVal = ParseStrength(pCard.name);
        int eVal = ParseStrength(eCard.name);

        if (pVal > eVal)
        {
            // Gracz wygrywa: zbierz karty, przenieœ je do playerDeckPos
            CollectCards(playerDeck, playerDeckPos);
            Debug.Log($"Gracz ({pCard.name}) pokonuje Komputer ({eCard.name})");
        }
        else
        {
            // Komputer wygrywa
            CollectCards(enemyDeck, enemyDeckPos);
            Debug.Log($"Komputer ({eCard.name}) pokonuje Gracza ({pCard.name})");
        }
    }

    void CollectCards(Queue<Transform> deck, Transform deckPos)
    {
        foreach (var card in warPile)
        {
            deck.Enqueue(card);
            card.position = deckPos.position;
            card.localScale = Vector3.one * winScale;
        }
        warPile.Clear();
    }

    int ParseStrength(string name)
    {
        string digits = new string(name.Where(char.IsDigit).ToArray());
        return int.TryParse(digits, out var v) ? v : 0;
    }
}
