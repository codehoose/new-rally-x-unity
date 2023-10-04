using System.Collections;
using UnityEngine;

public class CarFuelGauge : MonoBehaviour
{
    private static int POINTS_PER_FLAG = 100;
    private static int POINTS_PER_FLAG_BONUS = 200;

    private bool _paused;
    private int _currentPointsPerFlag = POINTS_PER_FLAG;
    private int _numFlags;
    private int _flagsCollected;
    private int _fuel = 63;
    private int _currentPlayer = 1;
    private int _score = 0;
    private int _currentRound = 1;

    [SerializeField]
    private Sidebar _sidebar;

    [SerializeField]
    private Locomotion _locomotion;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _sidebar.Score = 0;
        _sidebar.CurrentPlayer = _currentPlayer;
        _sidebar.CurrentRound = _currentRound;

        while (true)
        {
            if (!_paused)
            {
                _sidebar.Fuel = _fuel;
                yield return new WaitForSeconds(2f);
                _fuel--;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void PickUpFlag()
    {
        _flagsCollected++;
        _score += _currentPointsPerFlag * _flagsCollected;
        _sidebar.Score = _score;
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
        PickUpFlag();
        StartCoroutine(GatherFuel());
    }

    private IEnumerator GatherFuel()
    {
        _locomotion.Pause();
        _paused = true;

        int currentFuel = _fuel;
        while (currentFuel > 0)
        {
            currentFuel--;
            _score += 10; // TODO: HOW MUCH PER FUEL?
            _sidebar.Score = _score;
            _sidebar.Fuel = currentFuel;
            yield return new WaitForSeconds(0.125f);
        }

        _sidebar.Fuel = _fuel;
        _locomotion.Resume();
        _paused = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollectibleFlag flag = collision.GetComponent<CollectibleFlag>();
        if (flag == null) return;

        if (flag.FlagType == "Flag")
            PickUpFlag();
        else if (flag.FlagType == "Special")
            PickUpSpecialFlag();
        else if (flag.FlagType == "Lucky")
            PickUpLuckyFlag();

        _sidebar.RemoveFlag(flag.gameObject);
        Destroy(flag.gameObject);
    }
}
