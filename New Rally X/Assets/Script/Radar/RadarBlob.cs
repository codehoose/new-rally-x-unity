using System.Collections;
using UnityEngine;

public class RadarBlob : MonoBehaviour
{
    private int _frame;

    [SerializeField]
    private float _flashWait = 0.25f;

    [SerializeField]
    private Color[] _colors;

    private IEnumerator Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        while (true)
        {
            renderer.color = _colors[(_frame++) % _colors.Length];
            yield return new WaitForSeconds(_flashWait);
        }
    }

}
