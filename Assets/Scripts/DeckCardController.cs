using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckCardController : MonoBehaviour
{
    [System.Serializable]
    public class CustomDeckList
    {
        public int _damage;
        public int _defense;   
    }
    public CustomDeckList _deckList;
   
    public List<CustomDeckList> _deckListCards = new List<CustomDeckList>();

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

    public GameObject _cardPrefab;

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

            // 1. Creas una nueva instancia de tu clase
            CustomDeckList nuevaCarta = new CustomDeckList();

            // 2. Le asignas los valores a sus parámetros
            switch (HeroMain._heroScriptable[MainController.Instance._heroID]._startCards[i]._cardInfo[0]._type)
            {
                case CardsCard.CardInfo.Type.Attack:
                    nuevaCarta._damage = HeroMain._heroScriptable[MainController.Instance._heroID]._startCards[i]._cardInfo[0]._quantity;
                    break;
                case CardsCard.CardInfo.Type.Defense:
                    nuevaCarta._defense = HeroMain._heroScriptable[MainController.Instance._heroID]._startCards[i]._cardInfo[0]._quantity;
                    break;
            }        
           

            // 3. La agregas a tu lista
            _deckListCards.Add(nuevaCarta);
           
        }
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < _deckCards.Count; i++)
        {
            int randomIndex = Random.Range(i, _deckCards.Count);

            // 1. Barajamos los IDs de las cartas originales
            int tempDeck = _deckCards[i];
            _deckCards[i] = _deckCards[randomIndex];
            _deckCards[randomIndex] = tempDeck;

            // 2. Barajamos exactamente igual la lista de parámetros para que no pierdan la sincronía
            CustomDeckList tempCustom = _deckListCards[i];
            _deckListCards[i] = _deckListCards[randomIndex];
            _deckListCards[randomIndex] = tempCustom;
        }

        Debug.Log("Deck y lista de parámetros barajados correctamente.");
        SetCardsInHand();
    }
    public void SetCardsInHand()
    {
        for (int i = 0; i < 3; i++)
        {
            _handCards[i]._description.text = "+" + _allCards[_deckCards[i]]._cardInfo[0]._quantity.ToString() +
                _allCards[_deckCards[i]]._cardInfo[0]._type.ToString();
        }
    }
}