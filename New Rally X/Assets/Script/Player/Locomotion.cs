using System.Collections;
using UnityEngine;

public class Locomotion : MonoBehaviour
{
    [SerializeField]
    private Vector2 _position;

    IEnumerator Start()
    {
        transform.position = _position.ToWorldGrid();

        while (true)
        {
            Vector3 start = transform.position;
            Vector3 target = (_position + new Vector2(0, 1)).ToWorldGrid();
            float time = 0;
            while (time < 1f)
            {
                transform.position = Vector3.Lerp(start, target, time);
                time += Time.deltaTime;

                yield return null;
            }

            transform.position = target;
            _position = _position + new Vector2(0, 1);
        }
    }
}
