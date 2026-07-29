using System.Collections;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public static MainController Instance { get; private set; }
    public MapCreatorScript _mapScript;
    public PlayerMovementScript _scriptMovement;

    public GameObject _playerParent;
    public Camera _mainCamera;

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
    }

    void Update()
    {

    }

    public IEnumerator GameStarts()
    {
        yield return new WaitForSeconds(0.25f);
        _playerParent.transform.position = _mapScript._allMovementOrbs[0].transform.position;

    }
 
}