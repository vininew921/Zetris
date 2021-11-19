using GameJolt.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject pointsPanel;
    public GameObject controlsPanel;
    public GameObject nicknamePanel;
    public InputField nicknameInput;

    public void Start()
    {
        pointsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        nicknamePanel.SetActive(true);
    }

    public void StartGame()
    {
        pointsPanel.SetActive(true);
        controlsPanel.SetActive(true);
        nicknamePanel.SetActive(false);
    }

    public void ShowLeaderboards(int score, int level)
    {
        string scoreString = score.ToString() + " - " + level.ToString();
        GameJolt.API.Scores.Add(score, scoreString, nicknameInput.text, callback: (bool success) =>
        {
            GameJoltUI.Instance.ShowLeaderboards();
        });
    }
}
