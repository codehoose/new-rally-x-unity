using UnityEngine;

public static class MapPosition
{
    public static int GRID_SIZE = 24;
    public static int OFFSET = 12;

    public static Vector2 ToWorldGrid(this Vector2 gridPosition)
    {
        return new Vector2(OFFSET + gridPosition.x * GRID_SIZE, (gridPosition.y * GRID_SIZE) - OFFSET / 2);
    }

    public static Vector2 TestPosInDirection(this Vector2 position, Vector2 direction)
    {
        Vector2 offset = Vector2.zero;
        if (direction.x > 0)
            offset.x = 3;
        if (direction.x < 0)
            offset.x = -1;
        if (direction.y < 0)
            offset.y = -3;
        if (direction.y > 0)
            offset.y = 1;

        return offset + (position * 3);
    }
}
