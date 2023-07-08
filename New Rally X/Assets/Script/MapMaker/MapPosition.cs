using UnityEngine;

public static class MapPosition
{
    public static int GRID_SIZE = 24;
    public static int OFFSET = 12;

    public static Vector2 ToWorldGrid(this Vector2 gridPosition)
    {
        return new Vector2(OFFSET + gridPosition.x * GRID_SIZE, (gridPosition.y * GRID_SIZE) - OFFSET / 2);
    }
}
