using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Locomotion _player;

    [SerializeField]
    private EnemyLocomotion[] _enemies;

    [SerializeField]
    private float _gameDelay = 3f;

    [SerializeField]
    private float _enemyStartDelay = 1f;

    public EnemyLocomotion[] Enemies => _enemies;

    void Start()
    {
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
}
