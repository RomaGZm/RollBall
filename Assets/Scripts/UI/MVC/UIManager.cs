using RollBall.Core;
using UnityEngine;

public class UIManager : MonoBehaviour, IInit
{
    [SerializeField]
    private UIView _uIView;
    private UIModel _uIModel;
    private UIController _uIController;


    /// <summary>
    /// Initialization UI
    /// </summary>
    public void Init()
    {
        _uIModel = new UIModel();
        _uIController = new UIController(_uIModel, _uIView);
    }

    private void OnDestroy()
    {
        _uIController.Destroy();
    }
}
