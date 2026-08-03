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

    void Start()
    {
        _player = MainController.Instance._playerParent;
        _mapScript = MainController.Instance._mapScript;

        // Validamos que exista al menos el siguiente punto al iniciar
        if (_mapScript._allMovementOrbs != null && _mapScript._allMovementOrbs.Count > _onPos + 1)
        {
            _nextPoint = new Vector3(
                   _mapScript._allMovementOrbs[_onPos + 1].transform.position.x,
                   _mapScript._allMovementOrbs[_onPos + 1].transform.position.y + MainController.Instance._scriptHero._heroScriptable[MainController.Instance._heroID]._height / 2.5f,
                   _mapScript._allMovementOrbs[_onPos + 1].transform.position.z
               );
        }
    }

    void Update()
    {
        if (_nextPoint != null)
        {
            _distance = Vector3.Distance(_player.transform.position, _nextPoint);
        }

        if (Moving)
        {
            MoveCharVoid();
            ReachPoint();
            SlowChangingDirection();
        }

        if (_changingDirection)
        {
            ChangingDirection();
        }

        if (_playerAnimator != null)
        {
            _playerAnimator.SetBool("Moving", Moving);
        }
    }

    public void MoveCharVoid()
    {
        _player.transform.position =
            Vector3.MoveTowards(_player.transform.position,
            _nextPoint, _speed * Time.deltaTime);
    }

    public void ReachPoint()
    {
        if (_distance <= 0.2f && !_cantClick)
        {
            // Acabamos de llegar al beacon al que nos dirigíamos
            _onPos++;

            Debug.Log($"Beacon {_onPos}");

            // Cambia dirección si este beacon lo requiere
            if (_mapScript._allChangeDirections != null && _onPos < _mapScript._allChangeDirections.Count && _mapScript._allChangeDirections[_onPos])
            {
                StartCoroutine(RotationNumerator());
                Debug.Log("Cambia de dirección");
            }

            // ¿Existe otro beacon después? Si no existe, fin del recorrido.
            if (_mapScript._allMovementOrbs == null || _onPos + 1 >= _mapScript._allMovementOrbs.Count)
            {
                Debug.Log("Fin del recorrido");
                Moving = false;
                return;
            }

            // Preparar el siguiente destino
            _nextPoint = new Vector3(
                _mapScript._allMovementOrbs[_onPos + 1].position.x,
                _mapScript._allMovementOrbs[_onPos + 1].position.y +
                MainController.Instance._scriptHero._heroScriptable[MainController.Instance._heroID]._height / 2.5f,
                _mapScript._allMovementOrbs[_onPos + 1].position.z
            );

            // ================================================
            // CONDICIÓN DOBLE DE PARADA: 
            // 1. Debe tener _canHoldFight en true en su componente BeaconScript.
            // 2. Su índice (_onPos) debe estar enlistado en _allEnemiesPos.
            // ================================================
            bool canHoldFight = false;
            if (_mapScript._allMovementOrbs[_onPos].GetComponent<BeaconScript>() != null)
            {
                canHoldFight = _mapScript._allMovementOrbs[_onPos].GetComponent<BeaconScript>()._canHoldFight;
            }

            bool isEnemyAssigned = _mapScript._allEnemiesPos != null && _mapScript._allEnemiesPos.Contains(_onPos);

            if (canHoldFight && isEnemyAssigned)
            {
                Debug.Log("Batalla - Se detiene");
                Moving = false;
                _cantClick = true; // Bloquea clics por la batalla

                // ================================================
                // OBTENER EL MONSTRUO EXACTO DE ESTA BATALLA
                // ================================================
                int enemyIndexInList = _mapScript._allEnemiesPos.IndexOf(_onPos);
                if (enemyIndexInList != -1 && enemyIndexInList < _mapScript._allEnemiesCard.Count)
                {
                    Monster currentEnemyMonster = _mapScript._allEnemiesCard[enemyIndexInList];
                    Debug.Log($"¡Te enfrentas a: {currentEnemyMonster.name}!");
                    MainController.Instance._scriptBattle.SetEnemyInfo(currentEnemyMonster);

                    // Si necesitas enviarlo al controlador de batalla, puedes guardarlo en alguna variable global o pasarlo por parámetro:
                    // MainController.Instance._currentActiveEnemy = currentEnemyMonster;
                }

                StartCoroutine(MainController.Instance._scriptBattle.BattleStarts());
           
                return;
            }

            // Si no cumple ambas condiciones, el personaje pasa de largo automáticamente
            Moving = true;
            _cantClick = false;
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
        // Validación de seguridad para que no intente rotar si ya no hay siguiente punto
        if (_mapScript._allMovementOrbs == null || _onPos + 1 >= _mapScript._allMovementOrbs.Count)
            return;

        Transform player = MainController.Instance._playerParent.transform;
        Transform target = _mapScript._allMovementOrbs[_onPos + 1];

        if (target == null) return;

        Vector3 direction = target.position - player.position;
        direction.y = 0f;

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

    public void SlowChangingDirection()
    {
        // Validación de seguridad para que no intente rotar si ya no hay siguiente punto
        if (_mapScript._allMovementOrbs == null || _onPos + 1 >= _mapScript._allMovementOrbs.Count)
            return;

        Transform player = MainController.Instance._playerParent.transform;
        Transform target = _mapScript._allMovementOrbs[_onPos + 1];

        if (target == null) return;

        Vector3 direction = target.position - player.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            player.rotation = Quaternion.Slerp(
                player.rotation,
                targetRotation,
                (_rotationSpeed / 2) * Time.deltaTime
            );
        }
    }
}