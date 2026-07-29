using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MapCreatorScript : MonoBehaviour
{
    [Header("Map Assets")]
    public GameObject[] _mapAssets;

    [Header("Map Scriptables")]
    public Map[] _mapScriptables;

    [Header("Grid")]
    public int _gridWidth = 7;
    public int _gridHeight = 7;

    [Header("Map Settings")]
    public Vector3 _startPos;
    public float _separation = 20f;

    [Header("Direction Settings")]
    public float _directionSeparation = 1.5f;

    [Header("Spawned Maps")]
    public List<GameObject> _allSpawnedMaps = new List<GameObject>();

    public List<Transform> _allMovementOrbs;

    private void Start()
    {
        MapCreatorVoid();
    }

    public void CreateGrid()
    {
        if (_mapScriptables == null || _mapScriptables.Length == 0)
        {
            Debug.LogWarning("No hay Map Scriptables asignados.");
            return;
        }

        for (int i = 0; i < _mapScriptables.Length; i++)
        {
            if (_mapScriptables[i] == null)
                continue;

            _mapScriptables[i]._mapGrid.Clear();

            for (int z = 0; z < _gridHeight; z++)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    MapCell cell = new MapCell();

                    cell.x = x;
                    cell.z = z;
                    cell.mapID = -1;
                    cell.directionID = 0;

                    // IMPORTANTE:
                    // Antes tenías [0], ahora usamos [i]
                    _mapScriptables[i]._mapGrid.Add(cell);
                }
            }

      
        }
    }

    public void MapCreatorVoid()
    {
        ClearSpawnedMaps();

        if (_mapScriptables == null || _mapScriptables.Length == 0)
        {
            Debug.LogWarning("No hay Map Scriptables asignados.");
            return;
        }

        if (_mapAssets == null || _mapAssets.Length == 0)
        {
            Debug.LogWarning("No hay Map Assets asignados.");
            return;
        }



        for (int i = 0; i < _mapScriptables.Length; i++)
        {
            if (_mapScriptables[i] == null)
                continue;

            if (_mapScriptables[i]._mapGrid.Count == 0)
            {
                Debug.LogWarning(
                    $"La Grid del Map Scriptable {i} está vacía."
                );

                continue;
            }



            foreach (MapCell cell in _mapScriptables[i]._mapGrid)
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

                float _beaconPositionTolerance = 0.5f;

                MapBeacons beacons = map.GetComponent<MapBeacons>();

                foreach (Transform beacon in beacons._posBeacons)
                {
                    bool alreadyExists = false;

                    foreach (Transform existingBeacon in _allMovementOrbs)
                    {
                        float distance = Vector3.Distance(
                            beacon.position,
                            existingBeacon.position
                        );

                        // Ya existe un beacon prácticamente en la misma posición
                        if (distance <= _beaconPositionTolerance)
                        {
                            alreadyExists = true;
                            break;
                        }
                    }

                    // Solo agregar si no existe uno en esa posición
                    if (!alreadyExists)
                    {
                        _allMovementOrbs.Add(beacon);
                    }
                }
            }


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
        if (_mapScriptables == null || _mapScriptables.Length == 0)
        {
            return null;
        }

        for (int i = 0; i < _mapScriptables.Length; i++)
        {
            if (_mapScriptables[i] == null)
                continue;

            MapCell cell = _mapScriptables[i]._mapGrid.Find(
                cell => cell.x == x && cell.z == z
            );

            if (cell != null)
            {
                return cell;
            }
        }

        // No se encontró ninguna celda
        return null;
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