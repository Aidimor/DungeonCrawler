using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleController : MonoBehaviour
{
    public Animator _battlePanelAnimator;
    public Animator _enemyPanelAnimator;

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
        _enemyAssets._totalHp -= damage;
        //Debug.Log("Attack");
    }

    public void DefenseVoid(int shield)
    {
        _playerAssets._shieldPoints += shield;
        //Debug.Log("Defense");
    }
}
