using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileMapHelpers
{
    public static bool IsHit(this Tilemap tilemap, Vector3Int[] positions)
    {
        for(int i = 0; i < positions.Length; i++)
        {
            if (tilemap.GetTile(positions[i]) != null)
                return true;
        }

        return false;
    }
}