using UnityEngine;

public class SidebarText : MonoBehaviour
{
    private string _text;

    public Sprite[] font;

    public SpriteRenderer[] characters;

    public string text;

    public Color color;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (text != _text)
        {
            _text = text[..Mathf.Clamp(text.Length, text.Length, 8)];
            UpdateCharacters();
        }
    }

    private void UpdateCharacters()
    {
        for(int i = 0; i< characters.Length; i++)
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
