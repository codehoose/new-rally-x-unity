using System;
using System.Collections;
using UnityEngine;

public class CarFuelGauge : MonoBehaviour
{
    private static int POINTS_PER_FLAG = 100;
    private static int POINTS_PER_FLAG_BONUS = 200;

    private bool _paused;
    private int _currentPointsPerFlag = POINTS_PER_FLAG;
    private int _flagsCollected;
    private int _fuel = 63;
    private int _currentPlayer = 1;
    private int _score = 0;
    private int _currentRound = 1;
    private int _lives = 3;

    [SerializeField]
    private Sidebar _sidebar;

    [SerializeField]
    private Locomotion _locomotion;

    public int Score => _score;

    public int Lives => _lives;

    public int Round => _currentRound;

    public event EventHandler HitEnemy;

    public event EventHandler AllFlagsCollected;

    public event EventHandler FuelGathered;

    public event EventHandler NoMoreLives;

    public event EventHandler PauseEnemies;

    public event EventHandler ResumeEnemies;

    public void CollectRemainingFuel()
    {
        StartCoroutine(GatherFuel(false));
    }

    public void Init(int score, int lives, int round)
    {
        _score = score;
        _lives = lives;
        _currentRound = round;

        _sidebar.Score = _score;
        _sidebar.Lives = _lives;
        _sidebar.CurrentRound = _currentRound;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _sidebar.Score = _score;
        _sidebar.Lives = _lives;
        _sidebar.CurrentPlayer = _currentPlayer;
        _sidebar.CurrentRound = _currentRound;

        while (true)
        {
            if (!_paused)
            {
                _sidebar.Fuel = _fuel;
                
                float time = 0f;
                while (time < 1f)
                {
                    //float thrustTime = _locomotion.Thrust ? 0.5f : 1;
                    float thrustTime = _locomotion.Thrust ? 1 : 2;
                    time += Time.deltaTime / thrustTime;
                    yield return null;
                }

                _fuel = Math.Max(_fuel - 1, 0);
                _locomotion.Slowdown = _fuel == 0;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void PickUpFlag(bool luckyFlag = false)
    {
        _flagsCollected++;
        _score += _currentPointsPerFlag * _flagsCollected;
        _sidebar.Score = _score;
        if (!luckyFlag && _flagsCollected == 10)
        {
            AllFlagsCollected?.Invoke(this, EventArgs.Empty);
        }
    }

    private void PickUpSpecialFlag()
    {
        _flagsCollected++;
        _currentPointsPerFlag = POINTS_PER_FLAG_BONUS;
        _score += _currentPointsPerFlag * _flagsCollected;
        _sidebar.Score = _score;
    }

    private void PickUpLuckyFlag()
    {
        PauseEnemies?.Invoke(this, EventArgs.Empty);
        PickUpFlag(true);
        StartCoroutine(GatherFuel());
    }

    private IEnumerator GatherFuel(bool resumeGameplay = true)
    {
        _locomotion.Pause();
        _paused = true;

        int currentFuel = _fuel;
        while (currentFuel > 0)
        {
            currentFuel--;
            _score += 10;
            _sidebar.Score = _score;
            _sidebar.Fuel = currentFuel;
            yield return new WaitForSeconds(0.125f);
        }

        if (resumeGameplay)
        {
            _sidebar.Fuel = _fuel;
            _locomotion.Resume();
            ResumeEnemies?.Invoke(this, EventArgs.Empty);
            _paused = false;
        }
        else
        {
            FuelGathered?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollectibleFlag flag = collision.GetComponent<CollectibleFlag>();
        if (flag != null)
        {
            if (flag.FlagType == "Flag")
                PickUpFlag();
            else if (flag.FlagType == "Special")
                PickUpSpecialFlag();
            else if (flag.FlagType == "Lucky")
                PickUpLuckyFlag();

            _sidebar.RemoveFlag(flag.gameObject);
            Destroy(flag.gameObject);
        }
        else if (collision.tag == "Enemy")
        {
            EnemyLocomotion loco = collision.GetComponent<EnemyLocomotion>();
            if (loco.IsPaused) return;

            _lives--;
            _sidebar.Lives = Math.Max(_lives, 0);
            if (_lives > 0)
            {
                HitEnemy?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                NoMoreLives?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
