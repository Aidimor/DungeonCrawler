using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public static MainController Instance { get; private set; }
    public MapCreatorScript _mapScript;
    public PlayerMovementScript _scriptMovement;
    public HeroController _scriptHero;
    public BattleController _scriptBattle;
    public DeckCardController _scriptDeckController;
    public MonsterDatabase _scriptMonster;

    public Animator _cinematicAnimator;
    public GameObject _playerParent;
    public Camera _mainCamera;

    public int _heroID;

    public int _onStation; //0 = Nothing, 1 = DungeonMovement, 


    [System.Serializable]
    public class DungeonsMainInfo
    {
        public List<int> _DungeonIds = new List<int>();
        public bool _random;
  
    }
    public DungeonsMainInfo[] _dungeonsMainInfo;

    public DungeonCard[] _allDungeonCards;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(GameStarts());
        _scriptDeckController.StartDeckCreation();
        _scriptDeckController.ShuffleDeck();
        _scriptBattle.SetPlayerInfo(_scriptHero._heroScriptable[_heroID]);
    }

    void Update()
    {

    }

    public IEnumerator GameStarts()
    {
        yield return new WaitForSeconds(0.25f);
        _playerParent.transform.position = new Vector3(
                _mapScript._allMovementOrbs[0].transform.position.x,
                (_mapScript._allMovementOrbs[0].transform.position.y + (MainController.Instance._scriptHero._heroScriptable[MainController.Instance._heroID]._height / 2.5f)),
                _mapScript._allMovementOrbs[0].transform.position.z);

        yield return new WaitForSeconds(1);
        _onStation = 1;

    }
 
}