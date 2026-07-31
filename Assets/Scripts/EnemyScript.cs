using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform _playerTransform;
    

    void Start()
    {
        _playerTransform = MainController.Instance._playerParent.transform;
    }

    private void Update()
    {
        this.transform.LookAt(_playerTransform.transform.position);
    }

}
