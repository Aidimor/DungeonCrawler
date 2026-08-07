using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleController : MonoBehaviour
{
    public Animator _battlePanelAnimator;
    public Animator _enemyPanelAnimator;
    public Monster _currentMonster;
    public GameObject _monsterFightingObject;
    public GameObject _cardGiftPanel;

    [System.Serializable]
    public class EnemyAssets
    {
        public TextMeshProUGUI _name;
        public TextMeshProUGUI _hpText;
        public Image _fillImage;
        public int _maxHp;
        public int _totalHp;
        public float _fillAmmount;
    }
    public EnemyAssets _enemyAssets;

    [System.Serializable]
    public class PlayerAssets
    {
        public int _shieldPoints;
        public TextMeshProUGUI _shieldPointsText;
        public TextMeshProUGUI _name;
        public TextMeshProUGUI _hpText;
        public Image _fillImage;
        public int _maxHp;
        public int _totalHp;
        public float _fillAmmount;
   
    }
    public PlayerAssets _playerAssets;

    [System.Serializable]
    public class CardsOption
    {
        public GameObject _card;
        public TextMeshProUGUI _description;
        public CardsCard _cardInfo;
    }
    public CardsOption[] _cardsOption;

    public int _battleStation; //0 = Choose Action, 1 = Attack, 2 = EnemyAttack


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyPanelController();
        HeroPanelController();
    }

    public IEnumerator BattleStarts()
    {
        MainController.Instance._onStation = 0;
        MainController.Instance._cinematicAnimator.SetBool("CinematicIn", true);
        _enemyPanelAnimator.SetBool("EnemyIn", true);
        yield return new WaitForSeconds(1);
        _battlePanelAnimator.SetBool("BattleIn", true);
    }



    public void SetEnemyInfo(Monster currentEnemyMonster)
    {
        _enemyAssets._name.text = currentEnemyMonster.name;
        _enemyAssets._totalHp = currentEnemyMonster._hp;
        _enemyAssets._maxHp = currentEnemyMonster._hp;

    
    }

    public void SetPlayerInfo(HeroCard currentHero)
    {
        _playerAssets._name.text = currentHero.name;
        _playerAssets._totalHp = currentHero._hp;
        _playerAssets._maxHp = currentHero._hp;


    }


    public void HeroPanelController()
    {
        _playerAssets._fillAmmount = (float)_playerAssets._totalHp / (float)_playerAssets._maxHp;
        _playerAssets._fillImage.fillAmount = _playerAssets._fillAmmount;
        _playerAssets._hpText.text = _playerAssets._totalHp.ToString() + "/" + _playerAssets._maxHp.ToString();
        _playerAssets._shieldPointsText.text = _playerAssets._shieldPoints.ToString();
    }

    public void EnemyPanelController()
    {
        _enemyAssets._fillAmmount = (float)_enemyAssets._totalHp / (float)_enemyAssets._maxHp;
        _enemyAssets._fillImage.fillAmount = _enemyAssets._fillAmmount;
        _enemyAssets._hpText.text = _enemyAssets._totalHp.ToString() + "/" + _enemyAssets._maxHp.ToString();
       
    }

    public void CardChoosen(int id)
    {
        var Main = MainController.Instance._scriptDeckController;
        Debug.Log(Main._handCards[Main._deckCards[id]]._card.name);
        _battleStation = 0;
        StartCoroutine(BattleNumerator(id));
    }

    public IEnumerator BattleNumerator(int id)
    {
        var Main = MainController.Instance._scriptDeckController;
        switch (_battleStation)
        {
            case 0:
                _battlePanelAnimator.SetBool("BattleIn", false);
                switch (Main._allCards[Main._deckCards[id]]._cardInfo[0]._type)
                {
                    case CardsCard.CardInfo.Type.Attack:
                        AttackVoid(Main._allCards[Main._deckCards[id]]._cardInfo[0]._quantity);
                        break;
                    case CardsCard.CardInfo.Type.Defense:
                        DefenseVoid(Main._allCards[Main._deckCards[id]]._cardInfo[0]._quantity);
                        break;
                }
                if(_enemyAssets._totalHp == 0)
                {
                    GiftCardsChoose();
                    yield return new WaitForSeconds(1);
                    _cardGiftPanel.SetActive(true);
                    yield break;
                }
          
                    ThrowCardsVoid();
                    yield return new WaitForSeconds(2);
                    _battleStation = 1;
                    EnemyAttacks();
                    yield return new WaitForSeconds(1);
                    NewCardsVoid();
                
            
                break;
            case 1:
             
                break;
            case 2:
                break;
        }
        yield return null;
    }

    public void AttackVoid(int damage)
    {
      
        if(_enemyAssets._totalHp - damage <= 0)
        {
            _enemyAssets._totalHp = 0;
            MonsterKillVoid();
        }
        else
        {
            _enemyAssets._totalHp -= damage;
        }
    }

    public void DefenseVoid(int shield)
    {
        _playerAssets._shieldPoints += shield;     
    }

    public void EnemyAttacks()
    {
        var DamageDone = _currentMonster._attack;
        for (int i = 0; i < DamageDone; i++)
        {
            if(_playerAssets._shieldPoints > 0)
            {
                _playerAssets._shieldPoints--;
            }
            else
            {
                _playerAssets._totalHp--;
            }
        }
    }

    public void NewCardsVoid()
    {
        var deck = MainController.Instance._scriptDeckController;

        // Mientras no haya 3 cartas en la mano
        while (deck._deckCards.Count < 3 && deck._discardedCards.Count > 0)
        {
            int randomIndex = Random.Range(0, deck._discardedCards.Count);

            deck._deckCards.Add(deck._discardedCards[randomIndex]);
            deck._discardedCards.RemoveAt(randomIndex);
        }
        deck.SetCardsInHand();
        StartCoroutine(BattleStarts());
    
    }

    public void ThrowCardsVoid()
    {
        var DeckScript = MainController.Instance._scriptDeckController;
        for (int i = 0; i < 3; i++)
        {
            DeckScript._discardedCards.Add(DeckScript._deckCards[0]);
            DeckScript._deckCards.RemoveAt(0);
        }
    }

    public void GiftCardsChoose()
    {
        // Validación de seguridad por si el monstruo no tiene cartas configuradas
        if (_currentMonster._giftCard == null || _currentMonster._giftCard.Length == 0)
        {
            Debug.LogWarning("El monstruo no tiene GiftCards configuradas.");
            return;
        }

        // Creamos una lista temporal de las cartas para poder manipularlas (marcar como elegidas)
        List<Monster.GiftCard> availableCards = new List<Monster.GiftCard>(_currentMonster._giftCard);

        // Queremos seleccionar 2 opciones (o solo 1 si el monstruo tiene menos de 2 configuradas)
        int cantidadAElegir = Mathf.Min(2, availableCards.Count);

        Debug.Log("--- Seleccionando opciones de regalo ---");

        for (int i = 0; i < cantidadAElegir; i++)
        {
            // 1. Recalcular el total sumando únicamente los porcentajes de las opciones que AÚN NO han sido seleccionadas
            float totalChance = 0f;
            foreach (var gift in availableCards)
            {
                totalChance += gift._chace;
            }

            if (totalChance <= 0f) break;

            // 2. Tirar el dado aleatorio basado en el total actual
            float roll = Random.Range(0f, totalChance);
            float cumulativeProbability = 0f;
            int selectedIndex = -1;

            for (int j = 0; j < availableCards.Count; j++)
            {
                cumulativeProbability += availableCards[j]._chace;
                if (roll <= cumulativeProbability)
                {
                    selectedIndex = j;
                    break;
                }
            }

            // Fallback por seguridad de redondeo flotante
            if (selectedIndex == -1)
            {
                selectedIndex = availableCards.Count - 1;
            }

            // 3. Obtener la opción elegida y mostrarla en el Debug
            var chosenGift = availableCards[selectedIndex];
            string cardName = chosenGift._card != null ? chosenGift._card.name : "Carta sin asignar";
            _cardsOption[i]._cardInfo = availableCards[selectedIndex]._card;
            _cardsOption[i]._description.text = cardName;
            Debug.Log($"Opción {i + 1} elegida: {cardName} (Probabilidad acumulada/ajustada)");

            // 4. Remover la carta de la lista temporal para evitar que se repita en la siguiente vuelta
            availableCards.RemoveAt(selectedIndex);
        }
    }
    public void MonsterKillVoid()
    {
        StartCoroutine(_monsterFightingObject.GetComponent<EnemyScript>().DeadNumerator());
        
    }

    public void ChooseCardGift(int id)
    {
        MainController.Instance._scriptDeckController._discardedCards.Add(_cardsOption[id]._cardInfo._id);
    }

    public void EndsFight()
    {
        _cardGiftPanel.SetActive(false);
        _currentMonster = null;
        MainController.Instance._scriptMovement._cantClick = false;
        MainController.Instance._cinematicAnimator.SetBool("CinematicIn", false);
        _enemyPanelAnimator.SetBool("EnemyIn", false);
        MainController.Instance._onStation = 1;
    }
}
