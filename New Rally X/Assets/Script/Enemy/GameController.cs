using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Locomotion _player;

    [SerializeField]
    private CameraFollow _cameraFollow;

    [SerializeField]
    private EnemyLocomotion[] _enemies;

    [SerializeField]
    private float _gameDelay = 3f;

    [SerializeField]
    private float _enemyStartDelay = 1f;

    public EnemyLocomotion[] Enemies => _enemies;

    void Start()
    {
        _player.GetComponent<CarFuelGauge>().HitEnemy += Player_HitEnemy;
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

    private void Player_HitEnemy(object sender, EventArgs e)
    {
        foreach (var enemy in _enemies)
        {
            enemy.Crash();
        }

        _player.Crash();
        StartCoroutine(ResetPositions());
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
