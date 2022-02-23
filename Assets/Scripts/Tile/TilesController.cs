
using UnityEngine;
using RollBall.Core;

public class TilesController : MonoBehaviour, IInit
{
    [SerializeField]
    private PlayerMovement _playerMovement;
    [SerializeField]
    private TileSpawner _tileSpawner;
    [SerializeField]
    private PlayerData _playerData;
    [SerializeField]
    private GameManager _gameManager;

    private const int _TILES_BLOCK_AMOUNT = 5;
    private int _tilesCounter = 0;
    private Tile[] _tilesBlock = new Tile[_TILES_BLOCK_AMOUNT];

    /// <summary>
    /// Called when the player moves
    /// </summary>
    /// <param name="moveDirection"></param>
    /// <param name="currTile"></param>
    private void OnPlayerMovement(PlayerMovement.MoveDirection moveDirection, Tile currTile)
    {
        if (currTile != null && !currTile.IsAnimProcess)
        {
            currTile.Destroy( (_playerData.MoveSpeed * (int)_gameManager.Difficulty) / 5);
        }
        
       
    }
    /// <summary>
    /// Called when each tile is generated
    /// </summary>
    /// <param name="tile"></param>
    private void OnGenerateTile(Tile tile)
    {
         _tilesBlock[_tilesCounter] = tile;
         _tilesCounter++;
               
        if(_tilesCounter >= _TILES_BLOCK_AMOUNT)
        {
            _tilesBlock[Random.Range(1, 5)].GetComponent<Tile>().SetEnableCrystal(true);
            _tilesCounter = 0;
        }
       

    }
    private void OnDestroy()
    {
        _playerMovement.OnPlayerMovement -= OnPlayerMovement;
        _tileSpawner.GenerateTileEvent -= OnGenerateTile;
    }
    /// <summary>
    /// Initialization
    /// </summary>
    public void Init()
    {
        _playerMovement.OnPlayerMovement += OnPlayerMovement;
        _tileSpawner.GenerateTileEvent += OnGenerateTile;
    }
}
