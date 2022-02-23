using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public event Action<LevelDifficulty> ChoiceDifficultyLevel;
    public LevelDifficulty Difficulty = LevelDifficulty.Easy;

    [SerializeField]
    private TileSpawner _tileSpawner;
    [SerializeField]
    private TilesController _tilesController;
    [SerializeField]
    private PlayerMovement _playerMovement;
    [SerializeField]
    private UIManager _uIManager;

    public  enum LevelDifficulty
    {
        Easy = 3, Medium = 2, Hard = 1
    }


    private void Start()
    {
        _tileSpawner.GenerateTilesComplete += OnGenerateTilesComplete;
        _tilesController.Init();
        _tileSpawner.Init();
        _playerMovement.Init();
        _uIManager.Init();
        

    }
    /// <summary>
    /// Restarting the game
    /// </summary>
    public void RestartGame()
    {
        _tileSpawner.ResetTiles();
        _tileSpawner.RegenerateTiles();
        _playerMovement.Init();
        Time.timeScale = 1;
    }

    #region Events
    /// <summary>
    /// Called when tiles are generated.
    /// </summary>
    /// <param name="startTile"></param>
    /// <param name="lastTile"></param>
    private void OnGenerateTilesComplete(Tile startTile, Tile lastTile)
    {
        _playerMovement.transform.position = startTile.transform.position + new Vector3(0, 0.8f, 0);
    }
    /// <summary>
    /// Called when the difficulty level is selected
    /// </summary>
    /// <param name="levelDifficulty"></param>
    public void OnChoiceDifficultyLevel(int levelDifficulty)
    {
        Difficulty = (LevelDifficulty)levelDifficulty + 1;
     
        if (ChoiceDifficultyLevel != null)
        {
            ChoiceDifficultyLevel(Difficulty);
        }
    }

    private void OnDestroy()
    {
        _tileSpawner.GenerateTilesComplete -= OnGenerateTilesComplete;
    }

    #endregion
}
