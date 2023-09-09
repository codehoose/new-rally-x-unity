using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    private List<Vector2> _flags;

    [SerializeField]
    private TextAsset _jsonMap;

    [SerializeField]
    private Tilemap _tileMap;

    [SerializeField]
    private Sprite[] _blocks;

    [SerializeField]
    private Tile _tile;

    [SerializeField]
    public GameObject _flagPrefab;

    [SerializeField]
    public GameObject _specialFlagPrefab;

    public IList<Vector2> Flags => _flags;

    public bool IsReady { get; private set; }

    void Awake()
    {
        _flags = new List<Vector2>();
        UnityEngine.Random.InitState((int)System.DateTime.Now.Millisecond * 1000);

        MapFile mapFile = JsonUtility.FromJson<MapFile>(_jsonMap.text);
        MapLayer blocks = mapFile.layers.FirstOrDefault(layer => layer.name == "Blocks");
        MapLayer flags = mapFile.layers.FirstOrDefault(layer => layer.name == "Flags");
        flags.objects.Select(o => new Vector2(Mathf.FloorToInt(o.x / 24), Mathf.FloorToInt(-o.y / 24)))
                     .OrderBy(v => UnityEngine.Random.Range(0, 100))
                     .Take(10)
                     .ForEach(pos =>
                     {
                         _flags.Add(pos - new Vector2(4, -4));
                         Instantiate(_flagPrefab, MapPosition.SpriteToWorld(pos) + new Vector2(12, -12), Quaternion.identity);
                     });

        for (int y = -3; y < 192; y++)
        {
            for (int x = -6; x < 120; x++)
            {
                int blockIndexX = x < 0 ? (6 - Mathf.Abs(x)) % 3 : x;
                int blockIndexY = y < 0 ? blockIndexY = (3 - Mathf.Abs(y)) % 3 : y;

                int index = blocks.data[blockIndexY * 120 + blockIndexX] - 1;
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

        IsReady = true;
    }
}
