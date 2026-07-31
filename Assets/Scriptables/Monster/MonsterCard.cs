using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Scriptable Objects/Monster")]
public class Monster : ScriptableObject
{
    public string name;
    public Sprite portrait;
    public Texture _portraitTexture;
    public float _height;

    public Vector3 _pos;
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