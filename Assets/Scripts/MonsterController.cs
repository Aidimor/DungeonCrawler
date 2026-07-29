using UnityEngine;

public class MonsterDatabase : MonoBehaviour
{
    [SerializeField]
    private Monster[] monsters; 

    public Monster GetMonster(int index)
    {
        if (index < 0 || index >= monsters.Length)
            return null;

        return monsters[index];
    }

    public Monster GetMonster(string monsterName)
    {
        foreach (Monster monster in monsters)
        {
            if (monster.name == monsterName)
                return monster;
        }

        return null;
    }

    public void PrintAllMonsters()
    {
        foreach (Monster monster in monsters)
        {
            Debug.Log(monster.name);
        }
    }

    
}