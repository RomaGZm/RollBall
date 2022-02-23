using UnityEngine;

[CreateAssetMenu(fileName = "TileSpawnerData", menuName = "RollBall/TileSpawnerData", order = 1)]
public class TileSpawnerData : ScriptableObject
{

    public GameObject _tilePref;
    public int AmountPool = 20;
    public float TileSize = 1;
    public int AmountTilesAreaX = 3;
    public int AmountTilesAreaZ = 3;

}
