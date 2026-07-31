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
    public List<bool> _allCanStop = new List<bool>();

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
                    MainScript._dungeonsMainInfo[i]._DungeonIds.Add(Random.Range(0, _mapScriptables.Length));
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

    //    if (MainController.Instance == null)
    //    {
    //        Debug.LogWarning("MainController no existe.");
    //        return;
    //    }

    //    if (MainController.Instance._dungeonsMainInfo == null ||
    //        MainController.Instance._dungeonsMainInfo.Length == 0)
    //    {
    //        Debug.LogWarning("No hay DungeonsMainInfo.");
    //        return;
    //    }

    //    List<int> dungeonOrder =
    //        MainController.Instance._dungeonsMainInfo[0]._DungeonIds;


    //    if (dungeonOrder == null || dungeonOrder.Count == 0)
    //    {
    //        Debug.LogWarning("La lista de DungeonIds está vacía.");
    //        return;
    //    }


    //    //================================================
    //    // PUNTO INICIAL DEL PRIMER SET
    //    //================================================

    //    Vector3 nextSetStart = _startPos;
    //    Quaternion nextSetRotation = Quaternion.identity;



    //    //================================================
    //    // CREAR MAPAS
    //    //================================================

    //    for (int i = 0; i < dungeonOrder.Count; i++)
    //    {
    //        int mapIndex = dungeonOrder[i];


    //        if (mapIndex < 0 || mapIndex >= _mapScriptables.Length)
    //        {
    //            Debug.LogWarning($"Map Scriptable {mapIndex} no existe.");
    //            continue;
    //        }


    //        Map currentMap = _mapScriptables[mapIndex];


    //        if (currentMap == null)
    //        {
    //            Debug.LogWarning($"Map Scriptable {mapIndex} es NULL.");
    //            continue;
    //        }


    //        if (currentMap._mapGrid == null ||
    //            currentMap._mapGrid.Count == 0)
    //        {
    //            Debug.LogWarning($"Grid vacía en Map {mapIndex}");
    //            continue;
    //        }



    //        //================================================
    //        // CREAR PADRE DEL SET
    //        //================================================

    //        GameObject setParent = new GameObject($"Map Set {i}");

    //        setParent.transform.SetParent(transform);

    //        setParent.transform.position = nextSetStart;
    //        setParent.transform.rotation = nextSetRotation;
    //        setParent.transform.localScale = Vector3.one;



    //        MapBeacons firstMapBeacons = null;
    //        MapBeacons lastMapBeacons = null;



    //        foreach (MapCell cell in currentMap._mapGrid)
    //        {

    //            if (cell.mapID < 0)
    //                continue;


    //            if (cell.mapID >= _mapAssets.Length)
    //            {
    //                Debug.LogWarning(
    //                    $"MapID {cell.mapID} no existe."
    //                );

    //                continue;
    //            }



    //            //================================================
    //            // POSICION LOCAL DEL FRAGMENTO
    //            //================================================

    //            Vector3 localPosition = new Vector3(
    //                cell.x * _separation,
    //                0f,
    //                cell.z * _separation
    //            );


    //            Vector3 spawnPosition =
    //                nextSetStart +
    //                (nextSetRotation * localPosition);



    //            //================================================
    //            // ROTACION
    //            //================================================

    //            Quaternion spawnRotation =
    //                nextSetRotation *
    //                GetRotation(cell.directionID);



    //            //================================================
    //            // CREAR FRAGMENTO
    //            //================================================

    //            GameObject map = Instantiate(
    //                _mapAssets[cell.mapID],
    //                spawnPosition,
    //                spawnRotation,
    //                setParent.transform
    //            );


    //            _allSpawnedMaps.Add(map);



    //            MapBeacons beacons =
    //                map.GetComponent<MapBeacons>();


    //            if (beacons == null)
    //                continue;



    //            // Guardar primer y ultimo fragmento

    //            if (firstMapBeacons == null)
    //            {
    //                firstMapBeacons = beacons;
    //            }


    //            lastMapBeacons = beacons;



    //            foreach (MapBeacons.MainInfo info in beacons._mainInfo)
    //            {
    //                if (info._posBeacons == null)
    //                    continue;



    //                Transform beacon = info._posBeacons;


    //                bool alreadyExists = false;


    //                foreach (Transform existingBeacon in _allMovementOrbs)
    //                {
    //                    if (Vector3.Distance(
    //                        beacon.position,
    //                        existingBeacon.position)
    //                        <= _beaconTolerance)
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



    //        //================================================
    //        // ALINEAR EL START BEACON DEL NUEVO SET
    //        // CON EL END BEACON ANTERIOR
    //        //================================================

    //        if (i > 0 &&
    //            firstMapBeacons != null &&
    //            firstMapBeacons._startBeacon != null)
    //        {

    //            Vector3 offset =
    //                nextSetStart -
    //                firstMapBeacons._startBeacon.position;


    //            setParent.transform.position += offset;
    //        }



    //        //================================================
    //        // GUARDAR EL END DEL SET ACTUAL
    //        //================================================

    //        if (lastMapBeacons != null &&
    //            lastMapBeacons._endBeacon != null)
    //        {

    //            nextSetStart =
    //                lastMapBeacons._endBeacon.position;


    //            nextSetRotation =
    //                lastMapBeacons._endBeacon.rotation;
    //        }
    //    }
    //}

    public void MapCreatorVoid()
    {
        ClearSpawnedMaps();

        _allMovementOrbs.Clear();
        _allChangeDirections.Clear();
        _allCanStop.Clear();


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

        if (MainController.Instance == null)
        {
            Debug.LogWarning("MainController no existe.");
            return;
        }


        if (MainController.Instance._dungeonsMainInfo == null ||
            MainController.Instance._dungeonsMainInfo.Length == 0)
        {
            Debug.LogWarning("No hay DungeonsMainInfo.");
            return;
        }


        List<int> dungeonOrder =
            MainController.Instance._dungeonsMainInfo[0]._DungeonIds;



        if (dungeonOrder == null || dungeonOrder.Count == 0)
        {
            Debug.LogWarning("La lista de DungeonIds está vacía.");
            return;
        }



        Vector3 nextSetStart = _startPos;
        Quaternion nextSetRotation = Quaternion.identity;



        for (int i = 0; i < dungeonOrder.Count; i++)
        {

            int mapIndex = dungeonOrder[i];


            if (mapIndex < 0 || mapIndex >= _mapScriptables.Length)
            {
                Debug.LogWarning($"Map Scriptable {mapIndex} no existe.");
                continue;
            }



            Map currentMap = _mapScriptables[mapIndex];


            if (currentMap == null)
            {
                Debug.LogWarning($"Map Scriptable {mapIndex} es NULL.");
                continue;
            }



            if (currentMap._mapGrid == null ||
                currentMap._mapGrid.Count == 0)
            {
                Debug.LogWarning($"Grid vacía en Map {mapIndex}");
                continue;
            }



            GameObject setParent = new GameObject($"Map Set {i}");

            setParent.transform.SetParent(transform);

            setParent.transform.position = nextSetStart;
            setParent.transform.rotation = nextSetRotation;
            setParent.transform.localScale = Vector3.one;



            MapBeacons firstMapBeacons = null;
            MapBeacons lastMapBeacons = null;



            foreach (MapCell cell in currentMap._mapGrid)
            {

                if (cell.mapID < 0)
                    continue;


                if (cell.mapID >= _mapAssets.Length)
                {
                    Debug.LogWarning(
                        $"MapID {cell.mapID} no existe."
                    );

                    continue;
                }




                Vector3 localPosition = new Vector3(
                    cell.x * _separation,
                    0f,
                    cell.z * _separation
                );



                Vector3 spawnPosition =
                    nextSetStart +
                    (nextSetRotation * localPosition);



                Quaternion spawnRotation =
                    nextSetRotation *
                    GetRotation(cell.directionID);



                GameObject map = Instantiate(
                    _mapAssets[cell.mapID],
                    spawnPosition,
                    spawnRotation,
                    setParent.transform
                );



                _allSpawnedMaps.Add(map);



                MapBeacons beacons =
                    map.GetComponent<MapBeacons>();


                if (beacons == null)
                    continue;



                if (firstMapBeacons == null)
                {
                    firstMapBeacons = beacons;
                }


                lastMapBeacons = beacons;



                foreach (MapBeacons.MainInfo info in beacons._mainInfo)
                {

                    if (info._posBeacons == null)
                        continue;



                    Transform beacon = info._posBeacons;



                    _allMovementOrbs.Add(info._posBeacons);
                    _allChangeDirections.Add(info._changeDirections);
                    _allCanStop.Add(info._canStop);
                }
            }





            if (i > 0 &&
                firstMapBeacons != null &&
                firstMapBeacons._startBeacon != null)
            {

                Vector3 offset =
                    nextSetStart -
                    firstMapBeacons._startBeacon.position;


                setParent.transform.position += offset;
            }





            if (lastMapBeacons != null &&
                lastMapBeacons._endBeacon != null)
            {

                nextSetStart =
                    lastMapBeacons._endBeacon.position;


                nextSetRotation =
                    lastMapBeacons._endBeacon.rotation;
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