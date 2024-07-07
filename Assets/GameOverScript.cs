using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScript : MonoBehaviour
{
    public TextMeshProUGUI pointsText;

    public void Setup(int score) {
        Debug.Log("GameOverScript.Setup called with score: " + score); // Debug log
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " Points";
    }

    public void RestartButton() {
        SceneManager.LoadScene("SampleScene");
    }
}
