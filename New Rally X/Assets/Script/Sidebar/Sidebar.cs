using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sidebar : MonoBehaviour
{
    private const int MAX_LIVES = 3;

    private RadarBlob _playerDot;

    private RadarBlob[] _enemyDots;

    public SidebarText currentPlayer;

    public SidebarText currentRound;

    public SidebarScore highScore;

    public SidebarScore playerScore;

    public GameObject[] livesIndicator;

    public GameObject fuelIndicator;

    private Dictionary<GameObject, GameObject> _flagActualToRadar;

    [SerializeField]
    private GameObject _radar;

    [SerializeField]
    private RadarBlob _playerRadarPrefab;

    [SerializeField]
    private Locomotion _playerPosition;

    [SerializeField]
    private MapGenerator _mapGenerator;

    [SerializeField]
    private GameController _gameController;

    IEnumerator Start()
    {
        while (!_mapGenerator.IsReady) yield return null;

        _playerDot = Instantiate(_playerRadarPrefab, _radar.transform);
        _flagActualToRadar = new Dictionary<GameObject, GameObject>();

        _enemyDots = new RadarBlob[_gameController.Enemies.Length];
        for (int i = 0; i < _gameController.Enemies.Length; i++)
        {
            _enemyDots[i] = Instantiate(_playerRadarPrefab, _radar.transform);
            _enemyDots[i].SetColors(new Color(0.6f, 0, 0));
        }

        foreach (var flag in _mapGenerator.Flags)
        {
            RadarBlob radarBlob = Instantiate(_playerRadarPrefab, _radar.transform);
            radarBlob.transform.localPosition = flag.Position * 2f;
            _flagActualToRadar.Add(flag.RealFlag, radarBlob.gameObject);
            if (flag.FlagType == FlagType.Special)
            {
                radarBlob.SetColors(Color.yellow, Color.red);
            }
            else
            {
                radarBlob.SetColors(Color.yellow);
            }
        }

        while (true)
        {
            _playerDot.transform.localPosition = new Vector3(2f * (_playerPosition.GridPosition.x - 4), 2f * (_playerPosition.GridPosition.y + 4), 0);

            for (int i = 0; i < _gameController.Enemies.Length; i++)
            {
                EnemyLocomotion enemy = _gameController.Enemies[i];
                _enemyDots[i].transform.localPosition = new Vector3(2f * (enemy.GridPosition.x - 4), 2f * (enemy.GridPosition.y + 4), 0);
            }

            yield return null;
        }
    }

    public void RemoveFlag(GameObject flag)
    {
        Destroy(_flagActualToRadar[flag]);
    }

    public int Lives
    {
        set
        {
            int lives = Mathf.Clamp(value, 0, MAX_LIVES);

            for (int i =0; i < livesIndicator.Length; i++)
            {
                livesIndicator[i].SetActive(i < lives);
            }
        }
    }

    public int Fuel
    {
        set
        {
            float pc = Mathf.Clamp(value, 0, 63) / 63f;
            float y = fuelIndicator.transform.localScale.y;
            float z = fuelIndicator.transform.localScale.z;
            fuelIndicator.transform.localScale = new Vector3(Mathf.Clamp(pc, 0, 1f), y, z);
            fuelIndicator.GetComponent<SpriteRenderer>().color = value < 8 ? Color.red : Color.yellow;
        }
    }

    public int Score
    {
        set
        {
            playerScore.value = value;
        }
    }

    public int HiScore
    {
        set
        {
            highScore.value = value;
        }
    }

    public int CurrentPlayer
    {
        set
        {
            currentPlayer.text = $"{value}UP";
        }
    }

    public int CurrentRound
    {
        set
        {
            currentRound.text = $"ROUND {value}";
        }
    }
}
