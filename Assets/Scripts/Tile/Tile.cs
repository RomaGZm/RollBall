using System;
using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField]
    private Crystal _crystal;
    [SerializeField]
    private TileData _tileData;
    private bool _isDestroy = false;
    private Vector3 _tileSize = Vector3.one;

    public bool IsUsed = false;
    public bool IsAnimProcess { get; private set; } = false;
    public event Action OnDestroyCompliteEvent;

    public Vector3 TileSize
    {
        get
        {
            return _tileSize;
        }
        set
        {
            _tileSize = value;
            transform.localScale = _tileSize;
            _crystal.transform.localScale = new Vector3(_crystal._crystalData.crystalSize.x / _tileSize.x, _crystal._crystalData.crystalSize.y / _tileSize.y, _crystal._crystalData.crystalSize.z / _tileSize.z);

        }
    }
    /// <summary>
    /// Reset tile to default
    /// </summary>
    public void ResetTile()
    {
        IsAnimProcess = false;
        IsUsed = false;
        _isDestroy = false;

        SetEnableCrystal(false);
        StopAllCoroutines();
    }
    /// <summary>
    /// Animation destroy tile
    /// </summary>
    /// <param name="delay"></param>
    public void Destroy(float delay)
    {
        IsAnimProcess = true;
        StartCoroutine(Delay(delay));
    }
    /// <summary>
    /// Delay animation destroy
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator Delay(float delay)
    {

        yield return new WaitForSeconds(delay);
        _isDestroy = true;
        IsAnimProcess = false;
    }

    private void Update()
    {
        //Animation destroy tile
        if (_isDestroy)
        {
            float y = Mathf.MoveTowards(transform.position.y, _tileData.destroyAnimLen, Time.deltaTime * _tileData.destroyAnimSpeed);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);

            if (y <= _tileData.destroyAnimLen)
            {
                gameObject.SetActive(false);
                IsUsed = false;
                _isDestroy = false;

                DestroyComplete();
            }
        }

    }
    /// <summary>
    /// Complete animation destroy
    /// </summary>
    private void DestroyComplete()
    {
        if (OnDestroyCompliteEvent != null)
            OnDestroyCompliteEvent.Invoke();
    }
    /// <summary>
    /// Set enable/disable crystal
    /// </summary>
    /// <param name="enable"></param>
    public void SetEnableCrystal(bool enable)
    {
        _crystal.gameObject.SetActive(enable);
    }
    public void SetColor(Color color)
    {
        // GetComponent<Renderer>().material.SetColor("_Color", color);
    }
}
