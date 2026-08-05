using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckCardController : MonoBehaviour
{
    public CardsCard[] _allCards;
    public List<int> _deckCards = new List<int>();
    public List<int> _discardedCards = new List<int>();
    [System.Serializable]
    public class HandCards
    {
        public GameObject _card;
        public TextMeshProUGUI _description;
    }
    public HandCards[] _handCards;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDeckCreation()
    {
        var HeroMain = MainController.Instance._scriptHero.GetComponent<HeroController>();    
        for (int i = 0; i < HeroMain._heroScriptable[MainController.Instance._heroID]._startCards.Length; i++)
        {
            _deckCards.Add(HeroMain._heroScriptable[MainController.Instance._heroID]._startCards[i]._id);
        }
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < _deckCards.Count; i++)
        {
            int randomIndex = Random.Range(i, _deckCards.Count);
            int temp = _deckCards[i];
            _deckCards[i] = _deckCards[randomIndex];
            _deckCards[randomIndex] = temp;
        }

        Debug.Log("Deck barajado correctamente.");
        SetCardsInHand();
    }

    public void SetCardsInHand()
    {
        for(int i = 0; i < 3; i++)
        {
            _handCards[i]._description.text = "+" + _allCards[_deckCards[i]]._cardInfo[0]._quantity.ToString() +
                _allCards[_deckCards[i]]._cardInfo[0]._type.ToString();
        }
    }
}
