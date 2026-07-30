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
    public List<bool> _allChangeDirections;

    public float _beaconTolerance;

    private void Start()
    {

        StartMapsIdCreator();
        MapCreatorVoid();
    }

    public void StartMapsIdCreator()
    {
        var MainScript = MainController.Instance;       
        for(int i = 0; i < MainScript._dungeonsMainInfo.Length; i++)
        {
            if (MainScript._dungeonsMainInfo[i]._random)
            {
                for (int y = 0; y < MainScript._dungeonsMainInfo[i]._total; y++)
                {
                    MainScript._dungeonsMainInfo[i]._DungeonIds.Add(Random.Range(0, 2));
                }
            }
        
        }
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

    //public void MapCreatorVoid()
    //{
    //    ClearSpawnedMaps();

    //    if (_mapScriptables == null || _mapScriptables.Length == 0)
    //    {
    //        Debug.LogWarning("No hay Map Scriptables asignados.");
    //        return;
    //    }

    //    if (_mapAssets == null || _mapAssets.Length == 0)
    //    {
    //        Debug.LogWarning("No hay Map Assets asignados.");
    //        return;
    //    }



    //    for (int i = 0; i < _mapScriptables.Length; i++)
    //    {
    //        if (_mapScriptables[i] == null)
    //            continue;

    //        if (_mapScriptables[i]._mapGrid.Count == 0)
    //        {
    //            Debug.LogWarning(
    //                $"La Grid del Map Scriptable {i} está vacía."
    //            );

    //            continue;
    //        }



    //        foreach (MapCell cell in _mapScriptables[i]._mapGrid)
    //        {
    //            if (cell.mapID < 0)
    //                continue;

    //            if (cell.mapID >= _mapAssets.Length)
    //            {
    //                Debug.LogWarning(
    //                    $"MapID {cell.mapID} no existe en _mapAssets."
    //                );

    //                continue;
    //            }

    //            // ================================================
    //            // POSICIÓN BASE
    //            // ================================================

    //            Vector3 spawnPosition = _startPos + new Vector3(
    //                cell.x * _separation,
    //                0f,
    //                cell.z * _separation
    //            );

    //            // ================================================
    //            // OFFSET SEGÚN ORIENTACIÓN
    //            // ================================================

    //            switch (cell.directionID)
    //            {
    //                // 0 = Horizontal
    //                case 0:
    //                    break;

    //                // 1 = Vertical
    //                case 1:
    //                    spawnPosition.x += _directionSeparation;
    //                    break;

    //                default:
    //                    Debug.LogWarning(
    //                        $"Direction ID {cell.directionID} no válido en X:{cell.x} Z:{cell.z}."
    //                    );
    //                    break;
    //            }

    //            // ================================================
    //            // ROTACIÓN
    //            // ================================================

    //            Quaternion spawnRotation = GetRotation(cell.directionID);

    //            // ================================================
    //            // CREAR MAPA
    //            // ================================================

    //            GameObject map = Instantiate(
    //                _mapAssets[cell.mapID],
    //                spawnPosition,
    //                spawnRotation
    //            );

    //            map.transform.parent = transform;

    //            _allSpawnedMaps.Add(map);

    //            //float _beaconPositionTolerance = 1f;

    //            MapBeacons beacons = map.GetComponent<MapBeacons>();

    //            foreach (MapBeacons.MainInfo info in beacons._mainInfo)
    //            {
    //                if (info._posBeacons == null)
    //                    continue;

    //                Transform beacon = info._posBeacons;

    //                bool alreadyExists = false;

    //                foreach (Transform existingBeacon in _allMovementOrbs)
    //                {
    //                    if (Vector3.Distance(beacon.position, existingBeacon.position) <= _beaconTolerance)
    //                    {
    //                        alreadyExists = true;
    //                        break;
    //                    }
    //                }

    //                if (!alreadyExists)
    //                {
    //                    _allMovementOrbs.Add(beacon);
    //                    _allChangeDirections.Add(info._changeDirections);
    //                }
    //            }
    //        }


    //        }
    //}

    public void MapCreatorVoid()
    {
        ClearSpawnedMaps();

        _allMovementOrbs.Clear();
        _allChangeDirections.Clear();

        if (_mapAssets == null || _mapAssets.Length == 0)
        {
            Debug.LogWarning("No hay Map Assets asignados.");
            return;
        }

        var MainScript = MainController.Instance;
        if (MainScript == null || MainScript._dungeonsMainInfo == null || MainScript._dungeonsMainInfo.Length == 0)
        {
            Debug.LogWarning("No hay MainController o _dungeonsMainInfo asignados.");
            return;
        }

        // Posición inicial para el primer mapa
        Vector3 spawnPosition = _startPos;

        // Recorremos cada mazmorra/stage configurada en _dungeonsMainInfo
        for (int d = 0; d < MainScript._dungeonsMainInfo.Length; d++)
        {
            var dungeonInfo = MainScript._dungeonsMainInfo[d];

            if (dungeonInfo._DungeonIds == null || dungeonInfo._DungeonIds.Count == 0)
                continue;

            for (int i = 0; i < dungeonInfo._DungeonIds.Count; i++)
            {
                int mapID = dungeonInfo._DungeonIds[i];

                if (mapID < 0 || mapID >= _mapAssets.Length)
                {
                    Debug.LogWarning($"MapID {mapID} no existe en _mapAssets.");
                    continue;
                }

                int directionID = 0;

                // ================================================
                // OFFSET SEGÚN ORIENTACIÓN
                // ================================================
                Vector3 currentSpawnPos = spawnPosition;
                switch (directionID)
                {
                    case 0:
                        break;
                    case 1:
                        currentSpawnPos.x += _directionSeparation;
                        break;
                }

                // ================================================
                // ROTACIÓN
                // ================================================
                Quaternion spawnRotation = GetRotation(directionID);

                // ================================================
                // CREAR MAPA
                // ================================================
                GameObject map = Instantiate(
                    _mapAssets[mapID],
                    currentSpawnPos,
                    spawnRotation
                );

                map.transform.parent = transform;
                _allSpawnedMaps.Add(map);

                MapBeacons beacons = map.GetComponent<MapBeacons>();

                if (beacons != null)
                {
                    foreach (MapBeacons.MainInfo info in beacons._mainInfo)
                    {
                        if (info._posBeacons == null)
                            continue;

                        Transform beacon = info._posBeacons;
                        bool alreadyExists = false;

                        foreach (Transform existingBeacon in _allMovementOrbs)
                        {
                            if (Vector3.Distance(beacon.position, existingBeacon.position) <= _beaconTolerance)
                            {
                                alreadyExists = true;
                                break;
                            }
                        }

                        if (!alreadyExists)
                        {
                            _allMovementOrbs.Add(beacon);
                            _allChangeDirections.Add(info._changeDirections);
                        }
                    }
                }

                // Avanzamos la posición para el siguiente mapa
                spawnPosition += Vector3.right * _separation;
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