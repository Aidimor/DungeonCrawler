using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public bool Moving;
    public float _speed;
    public float _distance;
    GameObject _player;
    Transform _nextPoint;
    public int _onPos;
    public bool _cantClick;
    MapCreatorScript _mapScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = MainController.Instance._playerParent;
        _mapScript = MainController.Instance._mapScript;
        _nextPoint = _mapScript._allMovementOrbs[_onPos + 1];
    }

    // Update is called once per frame
    void Update()
    {
        if(_nextPoint != null)
        {
            _distance = Vector3.Distance(_player.transform.position, _nextPoint.transform.position);
        }
      
        if (Moving)
        {
            MoveCharVoid();
            ReachPoint();
        }
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
            _nextPoint.transform.position, _speed * Time.deltaTime);
        
    }

    public void ReachPoint()
    {
        if(_distance <= 0.2f && !_cantClick)
        {
            _onPos++;
            _nextPoint = _mapScript._allMovementOrbs[_onPos + 1];
            Debug.Log("Se detiene");         
            _cantClick = false;
            Moving = false;
            
        }
    }

    
}
