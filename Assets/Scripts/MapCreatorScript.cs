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
    public List<int> _allEnemies = new List<int>();

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

    public void SpawnEnemies()
    {
        _allEnemies.Clear();

        if (_allMovementOrbs == null || _allMovementOrbs.Count == 0)
        {
            Debug.LogWarning("No hay beacons (_allMovementOrbs) disponibles para generar enemigos.");
            return;
        }

        var dungeonCard = MainController.Instance._allDungeonCards[0];
        int totalBeacons = _allMovementOrbs.Count;
        int cantidadAGenerar = dungeonCard._totalEnemySpawns;

        // Si hay menos beacons que los que queremos generar, ajustamos el límite para evitar un bucle infinito
        if (cantidadAGenerar > totalBeacons)
        {
            cantidadAGenerar = totalBeacons;
            Debug.LogWarning("Hay menos beacons que la cantidad de enemigos solicitada. Se ajustará al total disponible.");
        }

        // Usamos un HashSet temporal para asegurar que los índices no se repiten
        HashSet<int> indicesUnicos = new HashSet<int>();

        while (indicesUnicos.Count < cantidadAGenerar)
        {
            int randomIndex = Random.Range(0, totalBeacons);
            indicesUnicos.Add(randomIndex);
        }

        // Pasamos el resultado a la lista pública y la ordenamos de menor a mayor
        _allEnemies = new List<int>(indicesUnicos);
        _allEnemies.Sort();

        // ================================================
        // SELECCIÓN E INSTANCIACIÓN DE ENEMIGOS EN LOS BEACONS
        // ================================================
        if (dungeonCard._enemies == null || dungeonCard._enemies.Length == 0)
        {
            Debug.LogWarning("No hay enemigos configurados en la DungeonCard.");
            return;
        }

        // Validamos que el prefab general de este script esté asignado
        if (_enemyPrefab == null)
        {
            Debug.LogError("No se ha asignado el _enemyPrefab en el inspector de este script.");
            return;
        }

        // Recorremos cada beacon seleccionado para convocar e instanciar al enemigo
        foreach (int beaconIndex in _allEnemies)
        {
            Monster summonedMonster = GetRandomEnemyByPercentage(dungeonCard._enemies);

            if (summonedMonster != null)
            {
                // Obtenemos el Transform del beacon usando el índice guardado
                Transform targetBeacon = _allMovementOrbs[beaconIndex];

                if (targetBeacon != null)
                {
                    // 1. Instanciamos el _enemyPrefab de este script en la posición y rotación del beacon
                    GameObject spawnedEnemy = Instantiate(
                        _enemyPrefab,
                        targetBeacon.position,
                        targetBeacon.rotation
                    );

                    // 2. Cambiamos la textura del material usando la instancia clonada (individual para este enemigo)
                    Renderer enemyRenderer = spawnedEnemy.GetComponent<Renderer>();
                    if (enemyRenderer != null && summonedMonster._portraitTexture != null)
                    {
                        enemyRenderer.material.SetTexture("_MainTex", summonedMonster._portraitTexture);
                    }

                    // 3. Asignamos a _enemiesParent como su contenedor/padre en la jerarquía
                    if (_enemiesParent != null)
                    {
                        spawnedEnemy.transform.SetParent(_enemiesParent);
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

    // Método auxiliar para calcular qué enemigo sale basado en el porcentaje (suma 100%)
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

        // Fallback por seguridad en caso de redondeo
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

