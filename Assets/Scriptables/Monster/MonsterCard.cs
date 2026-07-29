using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Scriptable Objects/Monster")]
public class Monster : ScriptableObject
{
    public string name;
    public Sprite portrait;
    public float _height;
}