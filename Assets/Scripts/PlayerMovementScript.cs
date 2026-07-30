using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public bool Moving;
    public float _speed;
    public float _distance;
    GameObject _player;
    Vector3 _nextPoint;
    public int _onPos;
    public bool _cantClick;
    MapCreatorScript _mapScript;
    public Animator _playerAnimator;
    public bool _changingDirection;
    public float _rotationSpeed;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = MainController.Instance._playerParent;
        _mapScript = MainController.Instance._mapScript;
        _nextPoint = new Vector3(
               _mapScript._allMovementOrbs[_onPos + 1].transform.position.x,
               _mapScript._allMovementOrbs[_onPos + 1].transform.position.y + MainController.Instance._scriptHero._heroScriptable[MainController.Instance._heroID]._height / 2.5f,
               _mapScript._allMovementOrbs[_onPos + 1].transform.position.z
           );
    }

    // Update is called once per frame
    void Update()
    {
        if(_nextPoint != null)
        {
            _distance = Vector3.Distance(_player.transform.position, _nextPoint);
        }
      
        if (Moving)
        {
            MoveCharVoid();
            ReachPoint();
        }

        if (_changingDirection)
        {
            ChangingDirection();
        }

        _playerAnimator.SetBool("Moving", Moving);
    }

    public IEnumerator MoveCharNumerator()
    {
        _cantClick = true;
        yield return new WaitForSeconds(1);
        _cantClick = false;
    }
    
    public void MoveCharVoid()
    {

        _player.transform.position =
            Vector3.MoveTowards(_player.transform.position,
            _nextPoint, _speed * Time.deltaTime);
        
    }

    public void ReachPoint()
    {
        if(_distance <= 0.2f && !_cantClick)
        {
            _onPos++;
            _nextPoint = new Vector3(
                _mapScript._allMovementOrbs[_onPos + 1].transform.position.x,
                _mapScript._allMovementOrbs[_onPos + 1].transform.position.y + MainController.Instance._scriptHero._heroScriptable[MainController.Instance._heroID]._height / 2.5f,
                _mapScript._allMovementOrbs[_onPos + 1].transform.position.z
            );

            if (_mapScript._allChangeDirections[_onPos])
            {
                StartCoroutine(RotationNumerator());
                Debug.Log("cambia de direccion");
            }

            Debug.Log("Se detiene");         
            _cantClick = false;
            Moving = false;
            
        }
    }

    public IEnumerator RotationNumerator()
    {
        _changingDirection = true;
        yield return new WaitForSeconds(1);
        _changingDirection = false;
    }

    public void ChangingDirection()
    {
        Transform player = MainController.Instance._playerParent.transform;
        Transform target = _mapScript._allMovementOrbs[_onPos + 1];

        Vector3 direction = target.position - player.position;
        direction.y = 0f; // Ignorar inclinación vertical

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            player.rotation = Quaternion.Slerp(
                player.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );
        }
    }


}
