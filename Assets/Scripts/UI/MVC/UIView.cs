using UnityEngine;
using UnityEngine.UI;
using System;

public class UIView : MonoBehaviour
{
    [SerializeField]
    private Text textTapToStart;
    [SerializeField]
    private Text textCrystalCount;
    [SerializeField]
    private GameManager _gameManager;

    public event Action OnResetCrystal;
    public ClickArea _clickArea;

    #region Events

    /// <summary>
    /// Reset crystal counter
    /// </summary>
    /// <param name="value"></param>
    private void ResetCrystal()
    {
        textCrystalCount.text = "0";
    }
    /// <summary>
    /// Text output when crystals are incremented
    /// </summary>
    /// <param name="crystalCounter"></param>
    public void TakeCrystal(int crystalCounter)
    {
        textCrystalCount.text = crystalCounter.ToString();
    }
    /// <summary>
    /// Start game event
    /// </summary>
    public void OnStartGame()
    {
        textTapToStart.gameObject.SetActive(false);
    }
    /// <summary>
    /// Button restart click event
    /// </summary>
    public void OnBtnRestartClick()
    {
        if (OnResetCrystal != null)
            OnResetCrystal.Invoke();
        ResetCrystal();

        _gameManager.RestartGame();
        gameObject.SetActive(false);
        textTapToStart.gameObject.SetActive(true);
        _clickArea.gameObject.SetActive(true);
    }
    /// <summary>
    /// Button exit click event
    /// </summary>
    public void OnBtnExittClick()
    {
        Application.Quit();
    }
    /// <summary>
    /// Game Over event
    /// </summary>
    public void OnGameOver()
    {
        gameObject.SetActive(true);
        textTapToStart.gameObject.SetActive(false);
        _clickArea.gameObject.SetActive(false);
    }
    #endregion
}
