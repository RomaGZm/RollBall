using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "RollBall/TileData", order = 1)]

public class TileData : ScriptableObject
{
    public float destroyAnimSpeed = 2;
    public float destroyAnimLen = -2;
    public float destroyDistTile = 1;
}
