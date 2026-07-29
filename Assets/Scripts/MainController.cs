using UnityEngine;

public class MainController : MonoBehaviour
{
    public static MainController Instance { get; private set; }

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

    }

    void Update()
    {

    }
}