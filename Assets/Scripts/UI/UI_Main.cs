using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Main : MonoBehaviour
{
    private bool gamePaused;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI totalCoinsText;

    private void Start()
    {
        SwitchMenu(mainMenu);
        Time.timeScale = 1;
        lastScoreText.text = "Last Score: " + PlayerPrefs.GetFloat("LastScore").ToString("#,#");
        highScoreText.text = "Best Score: " + PlayerPrefs.GetFloat("HighScore").ToString("#,#");
    }

    public void SwitchMenu(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);

        totalCoinsText.text = PlayerPrefs.GetInt("Coins").ToString("F0");
    }

    public void StartGame() => GameManager.instance.UnlockPlayer();

    public void PauseGame()
    {
        if (gamePaused)
        {
            Time.timeScale = 1;
            gamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            gamePaused = true;
        }
    }

    public void RestartGame() => GameManager.instance.RestartLevel();

}
