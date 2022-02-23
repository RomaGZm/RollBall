using System.Collections;
using UnityEngine;
using System;
using RollBall.Core;

public class PlayerMovement : MonoBehaviour, IPlayerMovement, IInit
{
    public enum MoveDirection
    {
       none, fwd, right
    }

    public MoveDirection moveDirection = MoveDirection.none;

    [SerializeField]
    private PlayerData _playerData;

    public event Action<MoveDirection, Tile> OnPlayerMovement;
    public static event Action OnGameOver;

    private Vector3 _direction = Vector3.zero;
    private GameObject current_tile;
    private bool _isGrounded = false;
    private bool _gameoverOneCall = true;
    private Vector3 _offsetRay = Vector3.zero;

    /// <summary>
    /// Setting the direction of travel
    /// </summary>
    /// <param name="moveDirection"></param>
    public void MoveTo(MoveDirection moveDirection)
    {
        this.moveDirection = moveDirection;
    }

  
    private void Update()
    {
        if (moveDirection == MoveDirection.none) return;

        RaycastHit hit;

        //Offset Ray in direction of travel  
        if (moveDirection == MoveDirection.right)
            _offsetRay = new Vector3(0.25f, 0, 0);
        else
            _offsetRay = new Vector3(0, 0, 0.25f);

        Debug.DrawRay(transform.position + _offsetRay, Vector3.down * 10, Color.blue);

        //Detection ground
        if (Physics.Raycast(transform.position + _offsetRay, Vector3.down * 10, out hit, 1, LayerMask.GetMask("Tile")))
        {
            current_tile = hit.transform.gameObject;
            _isGrounded = true;
        }
        else
        {
            current_tile = null;
            _isGrounded = false;

        }

        if (current_tile && OnPlayerMovement != null)
            OnPlayerMovement(moveDirection, current_tile.GetComponent<Tile>());

        //Is Grounded
        if (_isGrounded)
        {
            if (moveDirection == MoveDirection.fwd)
            {
                transform.Translate(Vector3.back * Time.deltaTime * _playerData.MoveSpeed);
               
            }
            else
            {
                transform.Translate(Vector3.left * Time.deltaTime * _playerData.MoveSpeed);
            }
        } 
        //Gravity
        else  
        {
            transform.Translate(Vector3.down * Time.deltaTime * _playerData.Gravity);
            if (_gameoverOneCall)
            {
                StartCoroutine(GameOver(0.5f));
                _gameoverOneCall = false;
            }
        }   
 
    }
    //Delay Game Over
    private IEnumerator GameOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0;
        if (OnGameOver != null)
        {
            OnGameOver.Invoke();
        }

    }
    //Initialization
    public void Init()
    {
        _isGrounded = false;
        _gameoverOneCall = true;
        moveDirection = MoveDirection.none;
    }
}
