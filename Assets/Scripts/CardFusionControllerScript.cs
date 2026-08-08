using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static DeckCardController;

public class CardFusionControllerScript : MonoBehaviour
{
    [System.Serializable]
    public class FusionAssets
    {
        public GameObject _parent;
        public GameObject _cardStart;
        public List<Vector2> _cardsPos = new List<Vector2>();
        public float _separation;
    }
    public FusionAssets _fusionAssets;




    [System.Serializable]
    public class TargetsInfo
    {
        public RectTransform _targetObject; // Arrastra aquí tu primer objeto del Canvas     
        public GameObject _cardObject;
        public int _attack;
        public int _defense;
        public bool _cardSet;
        public int _cardId;
    }
    public TargetsInfo[] _targetsInfo;

    public GameObject _cardObjectSelected;
    public Button _fusionButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetsInfo[0]._cardSet && _targetsInfo[1]._cardSet)
        {
            _fusionButton.gameObject.SetActive(true);
        }
        else
        {
            _fusionButton.gameObject.SetActive(false);
        }
    }

    public void FusionCardsVoid()
    {
        var Main = MainController.Instance;
        var Deck = Main._scriptDeckController.GetComponent<DeckCardController>();

        // 1. Creas una nueva instancia de tu clase
        CustomDeckList nuevaCarta = new CustomDeckList();
        for(int i = 0; i < 2; i++)
        {
            nuevaCarta._damage += _targetsInfo[i]._attack;
            nuevaCarta._defense += _targetsInfo[i]._defense;
        }
      
        Deck._deckListCards.Add(nuevaCarta);


    }

    public void SetCards()
    {
        var Main = MainController.Instance;
        var Deck = Main._scriptDeckController;
        int count = Deck._deckCards.Count;

        if (count == 0) return;

        // Definimos la separación base (250 para 8 cartas o menos)
        float separation = 200f;

        // Si hay más de 8 cartas, reducimos proporcionalmente la separación para que entren bien
        if (count > 8)
        {
            // Fórmula inversa: si suben las cartas, baja la separación proporcionalmente
            separation = 250f * (8f / (float)count);
        }

        for (int i = 0; i < count; i++)
        {
            GameObject Card = Instantiate(Deck._cardPrefab, transform.position, transform.rotation);
            Card.transform.parent = _fusionAssets._cardStart.transform;

            // Usamos la separación dinámica calculada
            Card.transform.localPosition = new Vector2(separation * i, 0);
            Card.GetComponent<UIDragHandler>()._startPos = new Vector2(separation * i, 0);
            Card.GetComponent<Button>().enabled = false;    
            Card.GetComponent<CardInfoScript>()._cardName.text = Deck._allCards[Deck._deckCards[i]]._cardInfo[0]._type.ToString();
            Card.GetComponent<CardInfoScript>()._attack += Deck._deckListCards[i]._damage;
            Card.GetComponent<CardInfoScript>()._defense += Deck._deckListCards[i]._defense;
            Card.GetComponent<CardInfoScript>()._id = i;
        }
    }
}
