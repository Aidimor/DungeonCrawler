using UnityEngine;

[CreateAssetMenu(fileName = "NewHero", menuName = "Scriptable Objects/Hero")]
public class HeroCard: ScriptableObject
{
    public string name;
    public Sprite portrait;
    public float _height;
}