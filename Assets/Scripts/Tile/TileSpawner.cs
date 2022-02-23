using RollBall.Core;
using RollBall.Core.Tile;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour, ITileSpawner, IInit
{

    [SerializeField]
    private TileSpawnerData _tileSpawnerData;
    public List<Tile> pooledObjects { get; private set; }

    public event Action<Tile, Tile> GenerateTilesComplete;
    public event Action<Tile> GenerateTileEvent;

    [SerializeField]
    private GameManager _gameManager;

    private Tile _tileCenterArea;
    private Tile[,] _tilesArea;
    private Tile _tileLast;

    /// <summary>
    /// Initialization
    /// </summary>
    public void Init()
    {
        int tileMult = (int)_gameManager.Difficulty;
        SpawnTiles((_tileSpawnerData.AmountPool * tileMult) + (_tileSpawnerData.AmountTilesAreaX * _tileSpawnerData.AmountTilesAreaZ));
        RegenerateTiles();

    }

    /// <summary>
    /// Regeneration of tile positions
    /// </summary>
    public void RegenerateTiles()
    {
        int tileMult = (int)_gameManager.Difficulty;
        ResetPoolObjects();
        GenerateTiles(_tileSpawnerData.AmountPool, _tileSpawnerData.TileSize * tileMult);
        GenerateTilesArea(_tileSpawnerData.AmountTilesAreaX, _tileSpawnerData.AmountTilesAreaZ, _tileSpawnerData.TileSize * tileMult);

        if (GenerateTilesComplete != null)
            GenerateTilesComplete.Invoke(_tileCenterArea, _tileLast);
    }
    /// <summary>
    /// Reset tiles to default
    /// </summary>
    public void ResetTiles()
    {
        foreach (Tile tile in pooledObjects)
        {
            tile.ResetTile();
        }
    }
    /// <summary>
    /// Adding Tile to the Scene
    /// </summary>
    /// <param name="pos"> Position</param>
    /// <param name="tilePref"> Tile Prefab</param>
    /// <returns></returns>
    public Tile SpawnTile(Vector3 pos, GameObject tilePref)
    {
        Tile tile = Instantiate(tilePref, pos, tilePref.transform.rotation).GetComponent<Tile>();
        tile.OnDestroyCompliteEvent += OnDestroyComplite;
        return tile;
    }
    /// <summary>
    /// Adding Tiles to the Scene
    /// </summary>
    /// <param name="poolLenght"> Tiles pool length</param>
    private void SpawnTiles(int poolLenght)
    {

        pooledObjects = new List<Tile>(poolLenght);

        for (int i = 0; i < poolLenght; i++)
        {
            Tile tile = SpawnTile(Vector3.zero, _tileSpawnerData._tilePref);
            tile.gameObject.SetActive(false);
            pooledObjects.Add(tile);
        }

    }
    /// <summary>
    /// Generation of starting tiles 3x3
    /// </summary>
    /// <param name="tilesX"></param>
    /// <param name="tilesZ"></param>
    /// <param name="tileSize"></param>
    private void GenerateTilesArea(int tilesX, int tilesZ, float tileSize)
    {
        _tilesArea = new Tile[tilesX, tilesZ];
        for (int x = 0; x < tilesX; x++)
        {
            for (int z = 0; z < tilesZ; z++)
            {
                Tile tile = GetPooledObject();
                tile.TileSize = new Vector3(tileSize, 1, tileSize);
                tile.transform.position = new Vector3(x * tileSize, 0, z * tileSize);
                tile.gameObject.SetActive(true);
                _tilesArea[x, z] = tile;
            }

        }
        _tileCenterArea = _tilesArea[tilesX - 1, tilesZ - 1];

    }

    /// <summary>
    /// Setting tile positions based on random
    /// </summary>
    /// <param name="tileCount"></param>
    /// <param name="tileSize"></param>    
    private void GenerateTiles(int tileCount, float tileSize)
    {
        Tile pre_tile = GetPooledObject();
        pre_tile.TileSize = new Vector3(tileSize, 1, tileSize);

        List<Tile> tiles = new List<Tile>(tileCount);

        for (int i = 0; i < tileCount; i++)
        {
            Tile tile = GenerateTile(tileSize, pre_tile);
            tiles.Add(tile);

            pre_tile = tile;
            tile.gameObject.SetActive(true);
            tiles.Add(tile);
        }
        _tileLast = tiles[tiles.Count - 1];
        _tileLast.GetComponent<Tile>().SetColor(Color.red);
    }
    /// <summary>
    /// Setting tile position based on random
    /// </summary>
    /// <param name="tileSize"></param>
    /// <param name="lastTile"></param>
    /// <returns></returns>
    private Tile GenerateTile(float tileSize, Tile lastTile)
    {

        Tile tile = GetPooledObject();
        tile.TileSize = new Vector3(tileSize, 1, tileSize);

        int rnd = UnityEngine.Random.Range(0, 2);

        if (rnd == 0)
            tile.transform.position = lastTile.transform.position - (Vector3.forward * tileSize);
        else
            tile.transform.position = lastTile.transform.position - (Vector3.right * tileSize);

        _tileLast = tile;
        tile.gameObject.SetActive(true);

        if (GenerateTileEvent != null)
            GenerateTileEvent.Invoke(_tileLast);

        return tile;
    }
    /// <summary>
    /// Tile position regeneration
    /// </summary>
    private void ReGenerateTile()
    {
        int tileMult = (int)_gameManager.Difficulty;
        GenerateTile(_tileSpawnerData.TileSize * tileMult, _tileLast);
    }
    /// <summary>
    /// Animation destroy tile area
    /// </summary>
    /// <param name="delay"></param>
    public void DestroyTilesArea(float delay)
    {
        foreach (Tile tile in _tilesArea)
        {
            tile.Destroy(delay);
        }
    }
    /// <summary>
    /// Reset the default tile pool
    /// </summary>
    private void ResetPoolObjects()
    {
        foreach (Tile tile in pooledObjects)
        {
            tile.gameObject.SetActive(false);
            tile.SetEnableCrystal(false);
            tile.IsUsed = false;
        }
    }
    /// <summary>
    /// Returns an object from the pool, if there is no free one, a new one is created
    /// </summary>
    /// <returns>Free tile from the pool</returns>
    private Tile GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            Tile tile = pooledObjects[i];

            if (!tile.IsUsed)
            {
                tile.IsUsed = true;
                return tile;
            }
        }
        return SpawnTile(Vector3.zero, _tileSpawnerData._tilePref);
    }

    #region Events
    /// <summary>
    /// Called at the end of a tile destruction animation
    /// </summary>
    private void OnDestroyComplite()
    {
        ReGenerateTile();
    }

    #endregion
    private void OnDestroy()
    {
        foreach (Tile tile in pooledObjects)
            tile.OnDestroyCompliteEvent -= OnDestroyComplite;
    }
}
