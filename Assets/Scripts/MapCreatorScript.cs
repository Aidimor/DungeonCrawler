using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MapBeacons;

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
    public List<int> _allEnemiesPos = new List<int>();
    public List<Monster> _allEnemiesCard = new List<Monster>();

    public float _beaconTolerance;

    public GameObject _enemyPrefab;
    public Transform _enemiesParent;

    private void Start()
    {

        StartMapsIdCreator();
        MapCreatorVoid();
        SpawnEnemies();
    }

    public void StartMapsIdCreator()
    {
        var MainScript = MainController.Instance;
        for (int i = 0; i < MainScript._dungeonsMainInfo.Length; i++)
        {
            if (MainScript._dungeonsMainInfo[i]._random)
            {
                for (int y = 0; y < MainScript._allDungeonCards[0]._totalRandomCreations; y++)
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
        _allEnemiesPos.Clear(); // Limpiamos también la lista de enemigos por seguridad


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
                    //_allCanStop.Add(info._canStop);
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

        // =========================================================
        // LLAMAMOS AQUÍ A LA GENERACIÓN DE ENEMIGOS UNA VEZ QUE 
        // TODOS LOS BEACONS DEL MAPA YA FUERON RECOLECTADOS
        // =========================================================
        SpawnEnemies();
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

    //    public void SpawnEnemies()
    //    {
    //        _allEnemies.Clear();

    //        if (_allMovementOrbs == null || _allMovementOrbs.Count == 0)
    //        {
    //            Debug.LogWarning("No hay beacons (_allMovementOrbs) disponibles para generar enemigos.");
    //            return;
    //        }

    //        int totalBeacons = _allMovementOrbs.Count;
    //        int cantidadAGenerar = MainController.Instance._allDungeonCards[0]._totalEnemySpawns;

    //        // Si hay menos beacons que los que queremos generar, ajustamos el límite para evitar un bucle infinito
    //        if (cantidadAGenerar > totalBeacons)
    //        {
    //            cantidadAGenerar = totalBeacons;
    //            Debug.LogWarning("Hay menos beacons que la cantidad de enemigos solicitada. Se ajustará al total disponible.");
    //        }

    //        // Usamos un HashSet temporal para asegurar que no se repitan los números
    //        HashSet<int> indicesUnicos = new HashSet<int>();

    //        while (indicesUnicos.Count < cantidadAGenerar)
    //        {
    //            int randomIndex = Random.Range(0, totalBeacons);
    //            indicesUnicos.Add(randomIndex);
    //        }

    //        // Pasamos el resultado a la lista pública y la ordenamos de menor a mayor
    //        _allEnemies = new List<int>(indicesUnicos);
    //        _allEnemies.Sort();
    //    }
    //}

    private bool _hasSpawnedEnemies = false;
    public void SpawnEnemies()
    {
        if (_hasSpawnedEnemies) return;
        _hasSpawnedEnemies = true;

        _allEnemiesPos.Clear();
        _allEnemiesCard.Clear();

        if (_allMovementOrbs == null || _allMovementOrbs.Count == 0)
        {
            Debug.LogWarning("No hay beacons (_allMovementOrbs) disponibles para generar enemigos.");
            return;
        }

        var dungeonCard = MainController.Instance._allDungeonCards[0];
        int cantidadAGenerar = dungeonCard._totalEnemySpawns;

        // ================================================
        // PASO 1: RECOPILAR ÚNICAMENTE LOS BEACONS VÁLIDOS (_canHoldFight == true)
        // ================================================
        List<int> beaconsValidos = new List<int>();

        for (int i = 0; i < _allMovementOrbs.Count; i++)
        {
            if (_allMovementOrbs[i].GetComponent<BeaconScript>() != null &&
                _allMovementOrbs[i].GetComponent<BeaconScript>()._canHoldFight)
            {
                beaconsValidos.Add(i);
            }
        }

        if (beaconsValidos.Count == 0)
        {
            Debug.LogWarning("No hay beacons disponibles con _canHoldFight activado para generar enemigos.");
            return;
        }

        if (cantidadAGenerar > beaconsValidos.Count)
        {
            cantidadAGenerar = beaconsValidos.Count;
            Debug.LogWarning("Hay menos beacons válidos con _canHoldFight que la cantidad de enemigos solicitada. Se ajustará al total disponible.");
        }

        // ================================================
        // PASO 2: SELECCIONAR ÍNDICES ÚNICOS DE LOS BEACONS VÁLIDOS
        // ================================================
        HashSet<int> indicesUnicos = new HashSet<int>();

        while (indicesUnicos.Count < cantidadAGenerar)
        {
            int randomValidIndex = Random.Range(0, beaconsValidos.Count);
            int actualBeaconIndex = beaconsValidos[randomValidIndex];
            indicesUnicos.Add(actualBeaconIndex);
        }

        _allEnemiesPos = new List<int>(indicesUnicos);
        _allEnemiesPos.Sort();

        // ================================================
        // PASO 3: SELECCIÓN E INSTANCIACIÓN DE ENEMIGOS EN LOS BEACONS
        // ================================================
        if (dungeonCard._enemies == null || dungeonCard._enemies.Length == 0)
        {
            Debug.LogWarning("No hay enemigos configurados en la DungeonCard.");
            return;
        }

        if (_enemyPrefab == null)
        {
            Debug.LogError("No se ha asignado el _enemyPrefab en el inspector de este script.");
            return;
        }

        foreach (int beaconIndex in _allEnemiesPos)
        {
            // Obtenemos el monstruo aleatorio basado en los porcentajes
            Monster summonedMonster = GetRandomEnemyByPercentage(dungeonCard._enemies);

            if (summonedMonster != null)
            {
                // Guardamos la referencia exacta del monstruo elegido en la lista
                _allEnemiesCard.Add(summonedMonster);

                Transform targetBeacon = _allMovementOrbs[beaconIndex];

                if (targetBeacon != null)
                {
                    GameObject spawnedEnemy = Instantiate(
                        _enemyPrefab,
                        targetBeacon.position,
                        targetBeacon.rotation
                    );

                    spawnedEnemy.GetComponent<EnemyScript>()._renderer.transform.localScale = summonedMonster._scale;

                    Renderer enemyRenderer = spawnedEnemy.GetComponent<EnemyScript>()._renderer.GetComponent<Renderer>();
                    if (enemyRenderer != null && summonedMonster._portraitTexture != null)
                    {
                        enemyRenderer.material.SetTexture("_MainTex", summonedMonster._portraitTexture);
                    }

                    if (_enemiesParent != null)
                    {
                        spawnedEnemy.transform.SetParent(_enemiesParent);
                        spawnedEnemy.GetComponent<EnemyScript>()._renderer.transform.localPosition =
                            new Vector3(
                                spawnedEnemy.GetComponent<EnemyScript>()._renderer.transform.localPosition.x,
                                spawnedEnemy.GetComponent<EnemyScript>()._renderer.transform.localPosition.y,
                                spawnedEnemy.GetComponent<EnemyScript>()._renderer.transform.localPosition.z
                            );
                    }
                    else
                    {
                        Debug.LogWarning("No se ha asignado _enemiesParent en el inspector. El enemigo se quedará sin padre en la raíz.");
                    }

                    Debug.Log($"Beacon [{beaconIndex}] -> Enemigo instanciado ({summonedMonster.name})");
                }
                else
                {
                    Debug.LogWarning($"Beacon [{beaconIndex}] -> El beacon es nulo.");
                }
            }
            else
            {
                Debug.Log($"Beacon [{beaconIndex}] -> Ningún enemigo seleccionado (revisa los porcentajes).");
            }
        }
    }

    private Monster GetRandomEnemyByPercentage(DungeonCard.Enemies[] enemiesList)
    {
        float roll = Random.Range(0f, 100f);
        float cumulativeProbability = 0f;

        foreach (var enemyData in enemiesList)
        {
            cumulativeProbability += enemyData._percentage;
            if (roll <= cumulativeProbability)
            {
                return enemyData._monsterCard;
            }
        }

        return enemiesList[enemiesList.Length - 1]._monsterCard;
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

