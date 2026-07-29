using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMap", menuName = "Scriptable Objects/Map")]
public class Map : ScriptableObject
{
    [Header("Grid")]
    public int _gridWidth = 7;
    public int _gridHeight = 7;

    [Header("Map Layout")]
    public List<MapCell> _mapGrid = new List<MapCell>();

}