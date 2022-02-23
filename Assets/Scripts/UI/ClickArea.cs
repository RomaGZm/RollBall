using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickArea : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement _playerMovement;
    [SerializeField]
    private PlayerData _playerData;
    [SerializeField]
    private TileSpawner _tileSpawner;

    private bool _moveDir = true;
    public static event Action OnStartGame;

    /// <summary>
    /// Changing the direction of the player's movement
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(BaseEventData eventData)
    {
        if (_playerMovement.moveDirection == PlayerMovement.MoveDirection.none) return;
        _moveDir = !_moveDir;

        if (_moveDir)
            _playerMovement.MoveTo(PlayerMovement.MoveDirection.fwd);
        else
            _playerMovement.MoveTo(PlayerMovement.MoveDirection.right);
    }
    /// <summary>
    /// Setting the player's movement when tapping
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(BaseEventData eventData)
    {
        if (_playerMovement.moveDirection == PlayerMovement.MoveDirection.none)
        {
            _playerMovement.MoveTo(PlayerMovement.MoveDirection.fwd);
            _tileSpawner.DestroyTilesArea(6);
            if (OnStartGame != null)
                OnStartGame.Invoke();
        }

    }


}
