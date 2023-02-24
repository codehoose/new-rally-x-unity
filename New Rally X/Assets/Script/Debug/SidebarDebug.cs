using UnityEngine;
using UnityEngine.UI;

public class SidebarDebug : MonoBehaviour
{
    public Sidebar sidebar;

    public Slider fuelSlider;
    public Slider scoreSlider;
    public Slider hiScoreSlider;
    public Slider roundsSlider;
    public Slider currentPlayerSlider;
    public Slider livesSlider;

    void Start()
    {
        fuelSlider.onValueChanged.AddListener((f) => sidebar.Fuel = (int)f);
        scoreSlider.onValueChanged.AddListener((f) => sidebar.Score = (int)f);
        hiScoreSlider.onValueChanged.AddListener((f) => sidebar.HiScore = (int)f);
        roundsSlider.onValueChanged.AddListener((f) => sidebar.CurrentRound = (int)f);
        currentPlayerSlider.onValueChanged.AddListener((f) => sidebar.CurrentPlayer = (int)f);
        livesSlider.onValueChanged.AddListener((f) => sidebar.Lives = (int)f);
    }
}
