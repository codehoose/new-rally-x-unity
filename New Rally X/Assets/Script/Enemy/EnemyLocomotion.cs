using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyLocomotion : MonoBehaviour
{
    private Vector2 _requestedDirection;

    private Vector2 _direction;

    private float _speed = 1f;

    private bool _paused;

    // The brains of the enemy car
    private Func<Vector2, Tilemap, Vector2, Vector2> FindNextDirection;

    [SerializeField]
    private Vector2 _position;

    [SerializeField]
    private Tilemap _tilemap;

    [SerializeField]
    private SpriteRenderer _sprite;

    [SerializeField]
    private Locomotion _player;

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

    void Start()
    {
        FindNextDirection = EnemyBrain.GetBrain(EnemyBrainType.Random);
        _paused = true;
        _direction = _requestedDirection = new Vector2(0, 1);
        transform.position = _position.SpriteToBlock() * 8; // SpriteToWorld();
    }

    public void StartTheEngine()
    {
        _paused = false;
        StartCoroutine(EngineRunning());
    }

    private IEnumerator EngineRunning()
    {
        yield return null;

        while (true)
        {
            Vector3 start = transform.position;
            Vector3 target = (_position + _direction).SpriteToBlock() * 8;
            float time = 0;
            _requestedDirection = _direction;
            _speed = 0.25f;
            while (time < 1f && !_paused)
            {
                transform.position = Vector3.Lerp(start, target, time);
                time += Time.deltaTime / _speed;
                yield return null;
            }

            transform.position = target;

            if (!_paused)
            {
                _position = _position + _direction;

                // Check blocks aren't in the way of the new direction
                Vector3Int[] positions = (_position + _requestedDirection).SpriteToBlock().GetTestPositions(_requestedDirection);

                if (_tilemap.IsHit(positions))
                {
                    _direction = FindNextDirection(_direction, _tilemap, _position);
                }
                else
                {
                    _direction = FindNextDirection(_requestedDirection, _tilemap, _position);
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
}
