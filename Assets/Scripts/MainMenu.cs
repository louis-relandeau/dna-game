using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI commentText;

    private Dictionary<int, string> scoreComments = new Dictionary<int, string>
    {
        { 20, "yh u can't do worse" },
        { 50, "not really trying are u" },
        { 100, "okay i guess better" },
        { 150, "even better" },
        { 200, "damn" },
        { int.MaxValue, "ninja" }
    };

    void Start() {
        MusicManager.Instance.SetMusicSpeed(1); // Reset
        MusicManager.Instance.SetMusicVolume(0.25f);
        MusicManager.Instance.PlayMusic(0);
        if (ScoreManager.Score.HasValue && ScoreManager.BestScore.HasValue) {
            int latestScore = ScoreManager.Score.Value;
            scoreText.text = "Previous score: " + latestScore.ToString()
                             + " (Best: " + ScoreManager.BestScore.Value.ToString() + ")";
            commentText.text = GetCommentFromScore(latestScore);
        } else {
            scoreText.text = "";
            commentText.text = "Welcome to the Main Menu!";
        }
    }

    private string GetCommentFromScore(int score)
    {
        foreach (var entry in scoreComments)
        {
            if (score < entry.Key)
            {
                return entry.Value;
            }
        }
        return "Nice try! Keep playing!";
    }

    public void PlayGame() {
        MusicManager.Instance.PlaySFX(2);
        SceneManager.LoadSceneAsync("MainLoop");
    }

    public void QuitGame() {
        MusicManager.Instance.PlaySFX(2);
        Application.Quit();
    }
}
