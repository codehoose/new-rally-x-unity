using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Locomotion : MonoBehaviour
{
    private Vector2 _requestedDirection;

    private Vector2 _direction;

    private float _speed = 1f;

    [SerializeField]
    private Vector2 _position;

    [SerializeField]
    private Tilemap _tilemap;

    [SerializeField]
    private SpriteRenderer _sprite;

    //private void OnDrawGizmos()
    //{
    //    Color current = Gizmos.color;
    //    Gizmos.color = Color.red;
    //    Vector3 testPos = _position.TestPosInDirection(_direction) * 8;
    //    Gizmos.DrawLine(testPos, testPos + new Vector3(24, 0));
    //    Gizmos.DrawLine(testPos, testPos + new Vector3(0, 24));
    //    Gizmos.DrawLine(testPos + new Vector3(24, 0), testPos + new Vector3(24, 24));
    //    Gizmos.DrawLine(testPos + new Vector3(0, 24), testPos + new Vector3(24, 24));
    //    Gizmos.color = current;
    //}

    IEnumerator Start()
    {
        _direction = _requestedDirection = new Vector2(0, 1);
        transform.position = _position.ToWorldGrid();

        while (true)
        {
            Vector3 start = transform.position;
            Vector3 target = (_position + _direction).ToWorldGrid();
            float time = 0;
            while (time < 1f)
            {
                transform.position = Vector3.Lerp(start, target, time);
                time += Time.deltaTime / _speed;

                _requestedDirection = _direction;
                _speed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 0.25f : 0.5f;

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    _requestedDirection = new Vector2(0, 1);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    _requestedDirection = new Vector2(0, -1);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    _requestedDirection = new Vector2(-1, 0);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    _requestedDirection = new Vector2(1, 0);
                }

                yield return null;
            }

            transform.position = target;
            _position = _position + _direction;

            // Check blocks aren't in the way of the new direction
            Vector2 blockPos = _position.TestPosInDirection(_requestedDirection);

            if (_tilemap.GetTile(new Vector3Int((int)blockPos.x, (int)blockPos.y, 0)) != null)
            {
                _direction = _requestedDirection * -1;
            }
            else
            {
                _direction = _requestedDirection;
            }

            _sprite.flipY = _direction.y < 0;
            float angle = 0;
            if (_direction.x < 0)
                angle = 90;
            else if (_direction.x > 0)
                angle = -90;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
