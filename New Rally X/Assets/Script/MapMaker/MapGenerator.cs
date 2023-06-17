using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    [SerializeField]
    private TextAsset _jsonMap;

    [SerializeField]
    private Tilemap _tileMap;

    [SerializeField]
    private Sprite[] _blocks;

    [SerializeField]
    private Tile _tile;

    void Awake()
    {
        MapFile mapFile = JsonUtility.FromJson<MapFile>(_jsonMap.text);
        for (int y = 0; y < 192; y++)
        {
            for (int x = 0; x < 120; x++)
            {
                int index = mapFile.layers[0].data[y * 120 + x] - 1;
                if (index >= 0)
                {
                    if (index >= 210 && index <= 212)
                        index = index - 6;
                    else if (index >= 231 && index <= 233)
                        index = index - 12;

                    _tile.sprite = _blocks[index % _blocks.Length];
                    _tileMap.SetTile(new Vector3Int(x, -y, 0), _tile);
                }
            }
        }
    }
}
