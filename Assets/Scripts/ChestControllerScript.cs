using UnityEngine;

public class ChestControllerScript : MonoBehaviour
{
    public GameObject _chestPrefab;

    [System.Serializable]
    public class Gifts
    {
        [System.Serializable]
        public enum Type
        {
            Gold,
            Card,
            Exp
        }
        public Type _type;
        [Range(0f, 100f)]  
        public float _chance;
    }
    public Gifts[] _gifts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
