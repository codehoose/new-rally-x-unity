using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Locomotion _player;

    [SerializeField]
    private CameraFollow _cameraFollow;

    [SerializeField]
    private EnemyLocomotion[] _enemies;

    [SerializeField]
    private GameObject _gameOver;

    [SerializeField]
    private float _gameDelay = 3f;

    [SerializeField]
    private float _enemyStartDelay = 1f;

    public EnemyLocomotion[] Enemies => _enemies;

    void Start()
    {
        CarFuelGauge gauge = _player.GetComponent<CarFuelGauge>();
        gauge.HitEnemy += Player_HitEnemy;
        gauge.AllFlagsCollected += Player_AllFlagsCollected;
        gauge.FuelGathered += Player_FuelGathered;
        gauge.NoMoreLives += Player_NoMoreLives;
        gauge.PauseEnemies += Player_PauseEnemies;
        gauge.ResumeEnemies += Player_ResumeEnemies;

        LoadPlayerData();
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(_gameDelay);
        _player.Resume();
        yield return new WaitForSeconds(_enemyStartDelay);
        foreach (var enemy in _enemies)
        {
            enemy.StartTheEngine();
        }
    }

    private void ResetPlayerData()
    {
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("lives", 3);
        PlayerPrefs.SetInt("round", 1);
    }

    private void SavePlayerData()
    {
        CarFuelGauge gauge = _player.GetComponent<CarFuelGauge>();
        PlayerPrefs.SetInt("score", gauge.Score);
        PlayerPrefs.SetInt("lives", gauge.Lives);
        PlayerPrefs.SetInt("round", gauge.Round);
    }

    private void LoadPlayerData()
    {
        CarFuelGauge gauge = _player.GetComponent<CarFuelGauge>();
        gauge.Init(PlayerPrefs.GetInt("score", 0),
                   PlayerPrefs.GetInt("lives", 3),
                   PlayerPrefs.GetInt("round", 1));
    }

    private void Player_ResumeEnemies(object sender, EventArgs e)
    {
        foreach (var enemy in _enemies)
        {
            enemy.Resume();
        }
    }

    private void Player_PauseEnemies(object sender, EventArgs e)
    {
        foreach (var enemy in _enemies)
        {
            enemy.Pause();
        }
    }

    private void Player_NoMoreLives(object sender, EventArgs e)
    {
        foreach (var enemy in _enemies)
        {
            enemy.Crash();
        }

        _player.Crash();
        _gameOver.SetActive(true);

        StartCoroutine(ReloadLevel());
    }

    private void Player_FuelGathered(object sender, EventArgs e)
    {
        SavePlayerData();
        SceneManager.LoadScene("NewRallyXGame");
    }

    private void Player_AllFlagsCollected(object sender, EventArgs e)
    {
        CarFuelGauge gauge = (CarFuelGauge)sender;
        foreach (var enemy in _enemies)
        {
            enemy.Pause();
        }

        gauge.CollectRemainingFuel();
    }

    private void Player_HitEnemy(object sender, EventArgs e)
    {
        foreach (var enemy in _enemies)
        {
            enemy.Crash();
        }

        _player.Crash();
        StartCoroutine(ResetPositions());
    }

    IEnumerator ReloadLevel()
    {
        ResetPlayerData();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("NewRallyXGame");
    }

    IEnumerator ResetPositions()
    {
        yield return new WaitForSeconds(2f);
        _player.Restart();
        _cameraFollow.Restart();

        yield return new WaitForSeconds(_enemyStartDelay);
        foreach (var enemy in _enemies)
        {
            enemy.Restart();
        }
    }
}
