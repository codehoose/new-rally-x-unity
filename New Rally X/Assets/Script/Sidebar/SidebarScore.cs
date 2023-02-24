using UnityEngine;

public class SidebarScore : MonoBehaviour
{
    private string _text;
    private int _value;

    public Sprite[] font;

    public SpriteRenderer[] characters;

    public int value;

    public Color color;

    private void Start()
    {
        _value = -1;
    }

    void Update()
    {
        if (value != _value)
        {
            _value = value;
            _text = string.Format("{0,8}", _value);
            _text = _text[..Mathf.Clamp(_text.Length, _text.Length, 8)];
            UpdateCharacters();
        }
    }

    private void UpdateCharacters()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (i < _text.Length)
            {
                characters[i].color = color;

                int ch = _text[i] - 'A';
                if (ch >= 0 && ch <= 26)
                {
                    characters[i].sprite = font[ch];
                }
                else if (_text[i] == ' ')
                {
                    characters[i].sprite = null;
                }
                else if (_text[i] == '!')
                {
                    characters[i].sprite = font[36];
                }
                else if (_text[i] == '"')
                {
                    characters[i].sprite = font[37];
                }
                else if (_text[i] == '.')
                {
                    characters[i].sprite = font[38];
                }
                else if (_text[i] == '-')
                {
                    characters[i].sprite = font[39];
                }
                else
                {
                    ch = _text[i] - '0'; // Integers start at 0 at index 26
                    characters[i].sprite = font[ch + 26];
                }
            }
            else
            {
                characters[i].sprite = null;
            }
        }
    }
}
