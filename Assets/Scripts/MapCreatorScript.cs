using UnityEngine;
using System.Collections.Generic;

public class MapCreatorScript : MonoBehaviour
{
    [Header("Map Assets")]
    public GameObject[] _mapAssets;

    [Header("Grid")]
    public int _gridWidth = 7;
    public int _gridHeight = 7;

    [Header("Map Settings")]
    public Vector3 _startPos;
    public float _separation = 20f;

    [Header("Direction Settings")]
    public float _directionSeparation = 1.5f;

    [Header("Map Layout")]
    public List<MapCell> _mapGrid = new List<MapCell>();

    [Header("Spawned Maps")]
    public List<GameObject> _allSpawnedMaps = new List<GameObject>();

    private void Start()
    {
        MapCreatorVoid();
    }

    public void CreateGrid()
    {
        _mapGrid.Clear();

        for (int z = 0; z < _gridHeight; z++)
        {
            for (int x = 0; x < _gridWidth; x++)
            {
                MapCell cell = new MapCell();

                cell.x = x;
                cell.z = z;
                cell.mapID = -1;
                cell.directionID = 0;

                _mapGrid.Add(cell);
            }
        }
    }

    public void MapCreatorVoid()
    {
        ClearSpawnedMaps();

        if (_mapGrid.Count == 0)
        {
            Debug.LogWarning("La Grid está vacía.");
            return;
        }

        if (_mapAssets == null || _mapAssets.Length == 0)
        {
            Debug.LogWarning("No hay Map Assets asignados.");
            return;
        }

        foreach (MapCell cell in _mapGrid)
        {
            if (cell.mapID < 0)
                continue;

            if (cell.mapID >= _mapAssets.Length)
            {
                Debug.LogWarning(
                    $"MapID {cell.mapID} no existe en _mapAssets."
                );

                continue;
            }

            // ================================================
            // POSICIÓN BASE
            // ================================================

            Vector3 spawnPosition = _startPos + new Vector3(
                cell.x * _separation,
                0f,
                cell.z * _separation
            );

            // ================================================
            // OFFSET SEGÚN ORIENTACIÓN
            // ================================================

            switch (cell.directionID)
            {
                // 0 = Horizontal
                case 0:
                    break;

                // 1 = Vertical
                case 1:
                    spawnPosition.x += _directionSeparation;
                    break;

                default:
                    Debug.LogWarning(
                        $"Direction ID {cell.directionID} no válido en X:{cell.x} Z:{cell.z}."
                    );
                    break;
            }

            // ================================================
            // ROTACIÓN
            // ================================================

            Quaternion spawnRotation = GetRotation(cell.directionID);

            // ================================================
            // CREAR MAPA
            // ================================================

            GameObject map = Instantiate(
                _mapAssets[cell.mapID],
                spawnPosition,
                spawnRotation
            );

            map.transform.parent = transform;

            _allSpawnedMaps.Add(map);
        }
    }

    private Quaternion GetRotation(int directionID)
    {
        switch (directionID)
        {
            // 0 = Horizontal
            case 0:
                return Quaternion.Euler(-90f, 0f, 0f);

            // 1 = Vertical
            case 1:
                return Quaternion.Euler(-90f, 0f, 90f);

            default:
                return Quaternion.identity;
        }
    }

    public void ClearSpawnedMaps()
    {
        for (int i = _allSpawnedMaps.Count - 1; i >= 0; i--)
        {
            if (_allSpawnedMaps[i] != null)
            {
                DestroyImmediate(_allSpawnedMaps[i]);
            }
        }

        _allSpawnedMaps.Clear();
    }

    public MapCell GetCell(int x, int z)
    {
        return _mapGrid.Find(cell =>
            cell.x == x &&
            cell.z == z
        );
    }
}

[System.Serializable]
public class MapCell
{
    public int x;
    public int z;

    // -1 = vacío
    // 0, 1, 2, 3... = MapAsset
    public int mapID = -1;

    // 0 = Horizontal
    // 1 = Vertical
    public int directionID = 0;
}