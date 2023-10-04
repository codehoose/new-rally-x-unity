using UnityEngine;

internal class FlagMapBlob
{
    public FlagType FlagType { get; set; }

    public Vector2 Position { get; set; }

    public GameObject RealFlag { get; set; }

    public FlagMapBlob(FlagType flagType, GameObject realFlag, Vector2 position)
    {
        FlagType = flagType;
        RealFlag = realFlag;
        Position = position;
    }
}