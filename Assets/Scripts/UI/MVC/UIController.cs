
public class UIController
{
    public UIModel _uIModel { get; set; }
    public UIView _uIView { get; set; }

    public UIController(UIModel uIModel, UIView uIView)
    {
        _uIModel = uIModel;
        _uIView = uIView;

        Crystal.OnTakeCrystal += OnTakeCrystal;
        ClickArea.OnStartGame += _uIView.OnStartGame;
        _uIView.OnResetCrystal += _uIModel.ResetCrystal;
        PlayerMovement.OnGameOver += _uIView.OnGameOver;

    }

    private void OnTakeCrystal()
    {
        _uIView.TakeCrystal(_uIModel.TakeCrystal());
    }
    public void Destroy()
    {
        Crystal.OnTakeCrystal -= OnTakeCrystal;
        ClickArea.OnStartGame -= _uIView.OnStartGame;
        _uIView.OnResetCrystal -= _uIModel.ResetCrystal;
        PlayerMovement.OnGameOver -= _uIView.OnGameOver;
    }
}
