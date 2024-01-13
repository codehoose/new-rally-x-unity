using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public static class EnemyBrain
{
    private static Vector2[] DIRECTIONS = new Vector2[] { Vector2.right, Vector2.down, Vector2.left, Vector2.up };

    public static Func<Vector2, Tilemap, Vector2, Vector2> GetBrain(EnemyBrainType type)
    {
        switch (type)
        {
            case EnemyBrainType.Random:
                return (direction, tilemap, position) =>
                {
                    int dirIndex = Array.IndexOf(DIRECTIONS, direction);
                    List<Vector2> directions = new List<Vector2>();
                    for (int i = 0; i < DIRECTIONS.Length; i++)
                    {
                        int actualIndex = (dirIndex + i) % DIRECTIONS.Length;
                        Vector2 dir = DIRECTIONS[actualIndex];
                        Vector3Int[] positions = (position + dir).SpriteToBlock().GetTestPositions(dir);
                        if (!tilemap.IsHit(positions))
                            directions.Add(dir);
                    }

                    if (directions.Contains(direction))
                    {
                        return direction;
                    }

                    return directions[Random.Range(0, directions.Count)];
                };
            default:
                return (direction, tilemap, position) => direction;
        }
    }
}
