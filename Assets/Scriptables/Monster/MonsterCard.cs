using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Scriptable Objects/Monster")]
public class Monster : ScriptableObject
{
    public string name;
    public int _hp;
    public Sprite portrait;
    public Texture _portraitTexture;
    public int _attack;
    //public float _height;
    //public float _distance;
    public Vector3 _scale;

    [System.Serializable]
    public enum Skills
    {
        NormalAttack,
        Defend,
        Special

    }
    public Skills[] _skills;
}