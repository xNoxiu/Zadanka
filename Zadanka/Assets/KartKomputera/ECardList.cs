using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Enemy Card List")]
public class ECardList : ScriptableObject
{
    public List<GameObject> cards;
}
