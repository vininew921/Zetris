using GameJolt.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject nicknamePanel;
    public InputField nicknameInput;

    public void Start()
    {
        mainPanel.SetActive(false);
        nicknamePanel.SetActive(true);
    }

    public void StartGame()
    {
        mainPanel.SetActive(true);
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
