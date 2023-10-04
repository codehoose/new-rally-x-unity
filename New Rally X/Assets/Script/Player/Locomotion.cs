using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Locomotion : MonoBehaviour
{
    private static Vector2[] DIRECTIONS = new Vector2[] { Vector2.right, Vector2.down, Vector2.left, Vector2.up };

    private Vector2 _requestedDirection;

    private Vector2 _direction;

    private float _speed = 1f;

    private bool _paused;

    [SerializeField]
    private Vector2 _position;

    [SerializeField]
    private Tilemap _tilemap;

    [SerializeField]
    private SpriteRenderer _sprite;

    public Vector2 GridPosition => _position;

    private void OnDrawGizmos()
    {
        Color current = Gizmos.color;
        Gizmos.color = Color.red;
        Vector3 pos = _position.SpriteToWorld();
        Gizmos.DrawLine(pos, pos + new Vector3(24, 0));
        Gizmos.DrawLine(pos, pos + new Vector3(0, -24));
        Gizmos.DrawLine(pos + new Vector3(24, 0), pos + new Vector3(24, -24));
        Gizmos.DrawLine(pos + new Vector3(0, -24), pos + new Vector3(24, -24));

        Gizmos.color = Color.green;
        pos = (_position + _direction).SpriteToWorld();
        Gizmos.DrawLine(pos, pos + new Vector3(24, 0));
        Gizmos.DrawLine(pos, pos + new Vector3(0, -24));
        Gizmos.DrawLine(pos + new Vector3(24, 0), pos + new Vector3(24, -24));
        Gizmos.DrawLine(pos + new Vector3(0, -24), pos + new Vector3(24, -24));

        Gizmos.color = current;
    }

    public void Pause() => _paused = true;

    public void Resume() => _paused = false;

    IEnumerator Start()
    {
        _direction = _requestedDirection = new Vector2(0, 1);
        transform.position = _position.SpriteToBlock() * 8; // SpriteToWorld();

        while (true)
        {
            Vector3 start = transform.position;
            Vector3 target = (_position + _direction).SpriteToBlock() * 8;
            float time = 0;
            while (time < 1f && !_paused)
            {
                transform.position = Vector3.Lerp(start, target, time);
                time += Time.deltaTime / _speed;

                _requestedDirection = _direction;
                _speed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 0.25f : 0.5f;

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    _requestedDirection = Vector2.up;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    _requestedDirection = Vector2.down;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    _requestedDirection = Vector2.left;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    _requestedDirection = Vector2.right;
                }

                yield return null;
            }

            if (!_paused)
            {
                transform.position = target;
                _position = _position + _direction;

                // Check blocks aren't in the way of the new direction
                Vector3Int[] positions = (_position + _requestedDirection).SpriteToBlock().GetTestPositions(_requestedDirection);

                if (_tilemap.IsHit(positions))
                {
                    _direction = FindNextDirection(_direction);
                }
                else
                {
                    _direction = FindNextDirection(_requestedDirection);
                }

                _sprite.flipY = _direction.y < 0;
                float angle = 0;
                if (_direction.x < 0)
                    angle = 90;
                else if (_direction.x > 0)
                    angle = -90;

                _sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                yield return null;
            }
        }
    }

    private Vector2 FindNextDirection(Vector2 direction)
    {
        /*
            DIRECTIONS:
            +-------+-------+-------+-------+
            |   0   |   1   |  2    |   3   |
            +-------+-------+-------+-------+
            | right | down  | left  |   up  |
            +-------+-------+-------+-------+

            Find 'dir' index the array e.g. if dir is 'left' then index is 2
            This function will then check the following indices in order: 2, 3, 0, 1
            because it wraps around the array using the % (mod) operator
         */

        int dirIndex = Array.IndexOf(DIRECTIONS, direction);
        for (int i = 0; i < DIRECTIONS.Length; i++)
        {
            int actualIndex = (dirIndex + i) % DIRECTIONS.Length;
            Vector2 dir = DIRECTIONS[actualIndex];
            Vector3Int[] positions = (_position + dir).SpriteToBlock().GetTestPositions(dir);
            if (!_tilemap.IsHit(positions))
                return dir;
        }

        return direction;
    }
}
