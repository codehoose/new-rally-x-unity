using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Sidebar : MonoBehaviour
{
    private const int MAX_LIVES = 3;

    private RadarBlob _playerDot;

    public SidebarText currentPlayer;

    public SidebarText currentRound;

    public SidebarScore highScore;

    public SidebarScore playerScore;

    public GameObject[] livesIndicator;

    public GameObject fuelIndicator;

    [SerializeField]
    private GameObject _radar;

    [SerializeField]
    private RadarBlob _playerRadarPrefab;

    [SerializeField]
    private Locomotion _playerPosition;

    IEnumerator Start()
    {
        _playerDot = Instantiate(_playerRadarPrefab, _radar.transform);

        while (true)
        {
            _playerDot.transform.localPosition = new Vector3(2f * (_playerPosition.GridPosition.x - 4), 2f * (_playerPosition.GridPosition.y + 4), 0);
            yield return null;
        }
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
