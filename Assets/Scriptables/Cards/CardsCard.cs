using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Scriptable Objects/Card")]
public class CardsCard: ScriptableObject
{
    public string _name;
    public int _id;

    [System.Serializable]
    public class CardInfo
    {
        [System.Serializable]
        public enum Type
        {
            Attack,
            Defense,
            Special
        }
        public Type _type;
        public int _quantity;
    }
    public CardInfo[] _cardInfo;


}