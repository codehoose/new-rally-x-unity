using System;
using System.Collections;
using UnityEngine;

public class RadarBlob : MonoBehaviour
{
    private int _frame;

    [SerializeField]
    private float _flashWait = 0.25f;

    [SerializeField]
    private Color[] _colors;

    public void SetColors(params Color[] colors)
    {
        _colors = new Color[colors.Length];
        Array.Copy(colors, _colors, colors.Length);
    }

    private IEnumerator Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        while (true)
        {
            renderer.color = _colors[_frame++ % _colors.Length];
            yield return new WaitForSeconds(_flashWait);
        }
    }
}
