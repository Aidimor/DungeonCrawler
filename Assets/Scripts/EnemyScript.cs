using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform _playerTransform;
    public GameObject _renderer;
    public ParticleSystem _explosionParticle;

    void Start()
    {
        _playerTransform = MainController.Instance._playerParent.transform;
    }

    private void Update()
    {
        _renderer.transform.LookAt(_playerTransform.transform.position);
    }

    public IEnumerator DeadNumerator()
    {
        _explosionParticle.Play();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

}
