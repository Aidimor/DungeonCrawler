using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    public Animator _battlePanelAnimator;   

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyPanelController();
    }

    public IEnumerator BattleStarts()
    {
        MainController.Instance._onStation = 0;
        MainController.Instance._cinematicAnimator.SetBool("CinematicIn", true);       
        yield return new WaitForSeconds(1);
        _battlePanelAnimator.SetBool("BattleIn", true);
    }

    public void CardChoosen(int id)
    {

    }

    public void SetEnemyInfo(Monster currentEnemyMonster)
    {
        _enemyAssets._name.text = currentEnemyMonster.name;
        _enemyAssets._totalHp = currentEnemyMonster._hp;
        _enemyAssets._maxHp = currentEnemyMonster._hp;

    
    }

    public void EnemyPanelController()
    {
        _enemyAssets._fillAmmount =  (float)_enemyAssets._totalHp / (float)_enemyAssets._maxHp;
        _enemyAssets._fillImage.fillAmount = _enemyAssets._fillAmmount;
        _enemyAssets._hpText.text = _enemyAssets._totalHp.ToString() + "/" + _enemyAssets._maxHp.ToString();
    }
}
