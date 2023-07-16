using UnityEngine;

public static class MapPosition
{
    public static int GRID_SIZE = 24;
    public static int BLOCKS_PER_SPRITE = 3;

    public static Vector2 SpriteToWorld(this Vector2 spritePosition)
    {
        return spritePosition.SpriteToBlock() * 8;
    }

    public static Vector2 SpriteToBlock(this Vector2 spritePosition)
    {
        return (spritePosition * BLOCKS_PER_SPRITE) + Vector2.up;
    }

    public static Vector3Int[] GetTestPositions(this Vector2 blockPosition, Vector2 direction)
    {
        if (direction.y < 0 || direction.y > 0)
        {
            blockPosition.y -= BLOCKS_PER_SPRITE;
            return GetVertical(blockPosition);
        }

        blockPosition.y--; // Fix for SpriteToBlock() compensation
        return GetHorizontal(blockPosition);
    }

    private static Vector3Int ToVector3Int(this Vector2 vector)
    {
        return new Vector3Int((int)vector.x, (int)vector.y, 0);
    }

    private static Vector3Int[] GetHorizontal(Vector2 blockPosition)
    {
        Vector3Int[] positions = new Vector3Int[3];
        for(int i =0; i < 3; i++)
        {
            positions[i] = (new Vector2(0, -i) + blockPosition).ToVector3Int();
        }

        return positions;
    }

    private static Vector3Int[] GetVertical(Vector2 blockPosition)
    {
        Vector3Int[] positions = new Vector3Int[3];
        for (int i = 0; i < 3; i++)
        {
            positions[i] = (new Vector2(i, 0) + blockPosition).ToVector3Int();
        }

        return positions;
    }
}
