using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform _playerTransform;
    public GameObject _renderer;

    void Start()
    {
        _playerTransform = MainController.Instance._playerParent.transform;
    }

    private void Update()
    {
        _renderer.transform.LookAt(_playerTransform.transform.position);
    }

}
