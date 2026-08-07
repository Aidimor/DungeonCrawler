using UnityEngine;

[CreateAssetMenu(fileName = "NewDungeon", menuName = "Scriptable Objects/Dungeon")]
public class DungeonCard: ScriptableObject
{
    public int _totalRandomCreations;
    public int _totalEnemySpawns;
    public int _totalRandomChests;
    [System.Serializable]
    public class Enemies
    {
        public Monster _monsterCard;

        [Range(0f, 100f)]
        public float _percentage;
       
    }
    public Enemies[] _enemies;
}