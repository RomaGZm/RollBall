
public class UIModel
{
    private int _crystalCounter = 0;

    /// <summary>
    /// Increment crystal counter
    /// </summary>
    /// <returns></returns>
    public int TakeCrystal()
    {
        _crystalCounter++;
        return _crystalCounter;

    }
    /// <summary>
    /// Reset crystal counter
    /// </summary>
    /// <returns></returns>
    public void ResetCrystal()
    {
        _crystalCounter = 0;
    }

}
