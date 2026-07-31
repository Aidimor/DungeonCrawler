using UnityEngine;

public class MapBeacons : MonoBehaviour
{
    [System.Serializable]
    public class MainInfo
    {
        public Transform _posBeacons;
        public bool _changeDirections;
        public bool _canStop;
    }
    public MainInfo[] _mainInfo;

    public Transform _startBeacon;
    public Transform _endBeacon;
    //public Transform[] _posBeacons;
    //public bool[] _changeDirections;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
